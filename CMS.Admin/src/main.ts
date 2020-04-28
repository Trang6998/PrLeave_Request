import '@babel/polyfill'
import Vue from 'vue';
import './plugins/vuetify'; 
import App from './App.vue';
import router from './router';
import store from './store/store';
import './registerServiceWorker';
import VeeValidate, { Validator } from "vee-validate";
import Snotify from 'vue-snotify';
import { SnotifyService } from 'vue-snotify/SnotifyService';
import * as moment from 'moment';
import DateTimePicker from '@/components/Commons/DateTimePicker';
import EventBus from '@/EventBus';
import CommonFunctions, { CommonFunctionsService } from './Utils/CommonFunctions';
Vue.use(DateTimePicker);
require('moment/locale/vi');

Vue.use(require('vue-moment'), {
    moment
});
Vue.use(EventBus);
Vue.use(CommonFunctions);
Vue.use(VeeValidate);
Validator.localize('vi');
Vue.use(Snotify);
Vue.config.productionTip = false;

declare module 'vue/types/vue' {
    interface Vue {
        $snotify: SnotifyService,
        $moment: any
    }
}
new Vue({
    router,
    store,
    render: (h) => h(App),
}).$mount('#app');
