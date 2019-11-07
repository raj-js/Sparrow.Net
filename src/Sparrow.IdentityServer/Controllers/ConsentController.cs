using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sparrow.IdentityServer.Models;
using Sparrow.IdentityServer.Models.Consent;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Sparrow.IdentityServer.Controllers
{
    /// <summary>
    /// 客户端可访问资源授权页面
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IEventService _events;
        private readonly ILogger<ConsentController> _logger;

        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService events,
            ILogger<ConsentController> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _events = events;
            _logger = logger;
        }

        /// <summary>
        /// 显示授权界面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await BuildViewModelAsync(returnUrl);

            if (vm == null) return View("Error");

            return View(vm);
        }

        /// <summary>
        /// 处理用户授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentFormModel model)
        {
            var result = await ProcessConsentAsync(model);

            if (result.IsRedirect)
            {
                if (await _clientStore.IsPkceClientAsync(result.ClientId))
                    return View("Redirect", new RedirectViewModel
                    {
                        RedirectUrl = result.RedirectUri
                    });

                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
                ModelState.AddModelError(string.Empty, result.ValidationError);

            if (result.ShowView)
                return View("Index", result.ViewModel);

            return View("Error");
        }

        #region privates

        /// <summary>
        /// 构建授权视图
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentFormModel model = null)
        {
            var authorizeRequest = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (authorizeRequest != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(authorizeRequest.ClientId);

                if (client != null)
                {
                    var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(authorizeRequest.ScopesRequested);

                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        return CreateConsentViewModel(model, returnUrl, client, resources);
                    }

                    _logger.LogError($"没有匹配的 Scope: {string.Join(",", authorizeRequest.ScopesRequested)}");
                }
                else
                {
                    _logger.LogError($"无效的客户端 Id: {authorizeRequest.ClientId}");
                }
            }
            else
            {
                _logger.LogError($"没有与授权请求相匹配的授权视图: {returnUrl}");
            }

            return null;
        }

        /// <summary>
        /// 创建授权视图模型
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <param name="client"></param>
        /// <param name="resources"></param>
        /// <returns></returns>
        private ConsentViewModel CreateConsentViewModel(
            ConsentFormModel model,
            string returnUrl,
            Client client,
            Resources resources
            )
        {
            var vm = new ConsentViewModel
            {
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),
                ReturnUrl = returnUrl,
                ClientName = client.ClientName,
                ClientUrl = client.ClientUri,
                ClientLogoUri = client.LogoUri,
                AllowRememberConsent = client.AllowRememberConsent
            };

            vm.IdentityResourcesScopes = resources.IdentityResources.Select(
                    resource => CreateIdentityScopeViewModel(resource, vm.ScopesConsented.Contains(resource.Name) || model == null)
                );

            vm.ApiResourcesScopes = resources.ApiResources.SelectMany(
                    resource => resource.Scopes.Select(
                        scope => CreateApiScopeViewModel(scope, vm.ScopesConsented.Contains(scope.Name) || model == null)
                        )
                );

            if (ConsentOptions.EnableOfflineAccess && resources.OfflineAccess)
            {
                vm.ApiResourcesScopes = vm.ApiResourcesScopes.Union(
                    new ScopeViewModel[]
                    {
                        GetOfflineAccessScopeViewModel(
                            vm.ScopesConsented.Contains(StandardScopes.OfflineAccess) || model == null
                            )
                    });
            }

            return vm;
        }

        /// <summary>
        /// 创建身份标识资源视图模型
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        private ScopeViewModel CreateIdentityScopeViewModel(IdentityResource resource, bool check)
        {
            return new ScopeViewModel
            {
                Name = resource.Name,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                Emphasize = resource.Emphasize,
                Required = resource.Required,
                Checked = check || resource.Required
            };
        }

        /// <summary>
        /// 创建 api 资源访问范围视图模型
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        private ScopeViewModel CreateApiScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required
            };
        }

        /// <summary>
        /// 获取离线访问权限视图模型
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        private ScopeViewModel GetOfflineAccessScopeViewModel(bool check)
        {
            return new ScopeViewModel
            {
                Name = StandardScopes.OfflineAccess,
                DisplayName = ConsentOptions.OfflineAccessDisplayName,
                Description = ConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
            };
        }

        /// <summary>
        /// 处理授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<ProcessConsentResult> ProcessConsentAsync(ConsentFormModel model)
        {
            var result = new ProcessConsentResult();

            var authorizationRequest = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (authorizationRequest == null) return result;

            ConsentResponse grantedConsent = null;

            if (model?.Button == "no")
            {
                grantedConsent = ConsentResponse.Denied;

                await _events.RaiseAsync(
                    new ConsentDeniedEvent(
                        User.GetSubjectId(),
                        authorizationRequest.ClientId,
                        authorizationRequest.ScopesRequested
                        )
                    );
            }
            else if (model?.Button == "yes")
            {
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    var scopes = model.ScopesConsented;

                    if (ConsentOptions.EnableOfflineAccess == false)
                        scopes = scopes.Where(scope => scope != StandardScopes.OfflineAccess);

                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = scopes.ToArray()
                    };

                    await _events.RaiseAsync(
                        new ConsentGrantedEvent(
                            User.GetSubjectId(),
                            authorizationRequest.ClientId,
                            authorizationRequest.ScopesRequested,
                            grantedConsent.ScopesConsented,
                            model.RememberConsent
                            )
                        );
                }
                else
                {
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                }
            }
            else
            {
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            }

            if (grantedConsent != null)
            {
                await _interaction.GrantConsentAsync(authorizationRequest, grantedConsent);

                result.RedirectUri = model.ReturnUrl;
                result.ClientId = authorizationRequest.ClientId;
            }
            else
            {
                result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
            }

            return result;
        }

        #endregion
    }
}