import { createApp } from 'vue';
import App from './App.vue';
import { createRouter, createWebHistory } from 'vue-router';
import CountryList from './components/CountryList.vue';
import CountryForm from './components/CountryForm.vue';

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: "/", name: "Home", component: CountryList},
        { path: "/country", name: "Country", component: CountryForm},
    ],
});

createApp(App).use(router).mount("#app");
