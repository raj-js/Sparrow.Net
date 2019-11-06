using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sparrow.IdentityServer.Core.Models;
using Sparrow.IdentityServer.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sparrow.IdentityServer.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        /// <summary>
        /// 登录流程入口
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var vm = await BuildLoginViewModelAsync(returnUrl);

            // 只允许第三方登录
            if (vm.IsExternalLoginOnly)
            {
                // 跳转至第三方登录质询
                return RedirectToAction("Challenge", "External", new { provider = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// 提交登录表单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginFormModel model, string button)
        {
            // 根据 returnUrl 获得认证请求
            var authorizeRequest = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // 若用户取消登录
            if (button != "login")
            {
                if (authorizeRequest != null)
                {
                    // 用户取消登录，返回OIDC访问被拒绝的响应给客户端
                    await _interaction.GrantConsentAsync(authorizeRequest, ConsentResponse.Denied);

                    if (await _clientStore.IsPkceClientAsync(authorizeRequest.ClientId))
                    {
                        return View("Redirect", new RedirectViewModel
                        {
                            RedirectUrl = model.ReturnUrl
                        });
                    }

                    return Redirect(model.ReturnUrl);
                }

                // 无效请求
                return Redirect("~/");
            }

            if (ModelState.IsValid)
            {
                // 实际上可能会为 UserName 登录， PhoneNo 登录， Email 登录
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

                    if (signInResult.Succeeded)
                    {
                        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

                        if (authorizeRequest != null)
                        {
                            if (await _clientStore.IsPkceClientAsync(authorizeRequest.ClientId))
                                return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });

                            return Redirect(model.ReturnUrl);
                        }

                        if (Url.IsLocalUrl(model.ReturnUrl))
                            return Redirect(model.ReturnUrl);
                        else if (string.IsNullOrEmpty(model.ReturnUrl))
                            return Redirect("~/");
                        else
                            throw new Exception("无效的 returnUrl");
                    }

                    // 是否需要双因素登录
                    if (signInResult.RequiresTwoFactor)
                        return RedirectToAction("LoginWith2fa", new { model.ReturnUrl, model.RememberMe });

                    // 用户已经锁定
                    if (signInResult.IsLockedOut)
                        return View("Lockout");
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.UserName, "无效的凭据"));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // 认证失败
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        /// <summary>
        /// 双因素登录
        /// </summary>
        /// <param name="rememberMe"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl)
        {
            // 确认用户已经经过了用户名密码验证
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
                throw new InvalidOperationException("无效的 2fa 登陆操作");

            var vm = new LoginWith2faViewModel
            {
                RememberMe = rememberMe,
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        /// <summary>
        /// 双因素登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
                throw new InvalidOperationException("无效的 2fa 登陆操作");

            var authenticatorCode = model.TwoFactorCode
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty);

            var signInResult = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.RememberMachine);

            if (signInResult.Succeeded)
                return LocalRedirect(model.ReturnUrl);

            if (signInResult.IsLockedOut)
                return View("Lockout");

            ModelState.AddModelError(string.Empty, "无效的认证码");
            return View(model);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
                return await Logout(vm);

            return View(vm);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutFormModel model)
        {
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            if (vm.TriggerExternalSignout)
            {
                var url = Url.Action("Logout", new { vm.LogoutId });
                return SignOut(new AuthenticationProperties
                {
                    RedirectUri = url
                }, vm.ExternalAuthenticationSchema);
            }

            return View("LoggedOut", vm);
        }

        #region privates

        /// <summary>
        /// 构建登录视图模型
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                var enableLocalLogin = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                var vm = new LoginViewModel
                {
                    EnableLocalLogin = enableLocalLogin,
                    ReturnUrl = returnUrl,
                    UserName = context?.LoginHint,
                };

                // 使用的是第三方登录
                if (!enableLocalLogin)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            (x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                )
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                UserName = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        /// <summary>
        /// 构建登录视图模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginFormModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.UserName = model.UserName;
            vm.RememberMe = model.RememberMe;
            return vm;
        }

        /// <summary>
        /// 构建登出视图模型
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel
            {
                LogoutId = logoutId,
                ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt
            };

            if (User?.Identity.IsAuthenticated != true)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var logoutRequest = await _interaction.GetLogoutContextAsync(logoutId);
            if (logoutRequest?.ShowSignoutPrompt == false)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            return vm;
        }

        /// <summary>
        /// 构建登出后的视图
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        public async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            var logoutRequest = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logoutRequest?.PostLogoutRedirectUri,
                ClientName = logoutRequest?.ClientName ?? logoutRequest?.ClientId,
                SignOutIFrameUrl = logoutRequest?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

                // 外部登录
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                    }
                    vm.ExternalAuthenticationSchema = idp;
                }
            }

            return vm;
        }

        #endregion
    }
}