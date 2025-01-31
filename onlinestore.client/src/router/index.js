import { createRouter, createWebHistory } from 'vue-router';

//layouts
import OnlineStore from '../layouts/OnlineStore.vue'
import EmptyLayout from '../layouts/EmptyLayout.vue'

//views
import About from '../views/AboutView.vue'
import Basket from '../views/BasketView.vue'
import Catalog from '../views/CatalogView.vue'
import Login from '../views/LoginView.vue'
import Orders from '../views/OrdersView.vue'
import Register from '../views/RegisterView.vue'
import Users from '../views/UsersView.vue'

const routes = [
  {
    path: '/',
    component: OnlineStore,
    children: [
      {
        path: '',
        name: 'Default',
        component: Catalog
      },
      {
        path: 'catalog',
        name: 'Catalog',
        component: Catalog
      },
      {
        path: 'orders',
        name: 'Orders',
        component: Orders
      },
      {
        path: 'users',
        name: 'Users',
        component: Users
      },
      {
        path: 'basket',
        name: 'Basket',
        component: Basket
      },
      {
        path: 'about',
        name: 'About',
        component: About
      }
    ]
  },
  {
    path: '/auth/',
    component: EmptyLayout,
    children: [
      {
        path: '',
        name: 'Login',
        component: Login
      },
      {
        path: 'new',
        name: 'Register',
        component: Register
      }
    ]
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = localStorage.getItem('jwt') !== null;
  if (!isAuthenticated && to.path !== '/auth' && to.path !== '/auth/new') {
    next('/auth');
  } else {
    next(); // Продолжаем навигацию
  }
});


export default router;
