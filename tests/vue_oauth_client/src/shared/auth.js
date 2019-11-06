const BASE_URL = 'https://localhost:5001'

export default {
    urls:{
        authorize: `${BASE_URL}/connect/authorize`
    },
    config: {
        client_id: 'vue_oauth_client',
        redirect_uri: '',
        scope: 'openid profile',
        state: '',
        allow_signup: false
    },
    services: {
        authorize: () => {
            
        }
    }
}