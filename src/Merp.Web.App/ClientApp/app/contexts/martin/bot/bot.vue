<template>
    <section class="web-chat">
        <div id="botframework-chat" />
    </section>
</template>
<style>
    .web-chat {
        width: 100%;
        height: 100%;
        min-height: 400px;
        overflow: hidden;
    }

        .web-chat #botframework-chat {
            width: 100%;
            height: 100%;
            min-height: 400px;
            overflow: hidden;
        }
</style>
<script>
    import * as BotChat from 'botframework-webchat'
    // import 'botframework-webchat/botchat.css'

    export default {
        name: 'web-chat',
        components: {},
        props: [ 'directLineSecret' ],
        data() {
            return {
                currentUserName: ''
            }
        },
        computed: {

        },
        async mounted() {
            let self = this
            let user = await this.$identity.getUser()

            self.currentUserName = user.profile.name

            const directLine = BotChat.createDirectLine({
                secret: 'tt5zxlV94tI.9iG-EIGo8TUVq-E1-HS-LGifgugQJ52cuVtZCS5MNWU',
                webSocket: false
            })

            // let webSpeechPonyfillFactory = BotChat.createBrowserWebSpeechPonyfillFactory()
            let webSpeechPonyfillFactory = await BotChat.createCognitiveServicesSpeechServicesPonyfillFactory({
                region: 'westeurope',
                subscriptionKey: '18d1d2bd948d472aaf374dd9f7e9bb20'
            })

            BotChat.renderWebChat({
                directLine: directLine,
                userID: user.profile.name,
                webSpeechPonyfillFactory: webSpeechPonyfillFactory
            }, document.getElementById("botframework-chat"))
        },
        methods: {

        }
    }
</script>