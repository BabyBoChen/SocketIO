import { createRouter, createWebHashHistory } from 'vue-router';
import Home from '@/views/Home.vue';
import Chatroom from '../views/Chatroom.vue';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/chatroom',
    name: 'Chatroom',
    component: Chatroom
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
