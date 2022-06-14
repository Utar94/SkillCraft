import Vue from 'vue'
import VueRouter from 'vue-router'
import store from './store'
import AspectList from './components/Aspects/AspectList.vue'
import Confirm from './components/Identity/Confirm.vue'
import CustomizationEdit from './components/Customizations/CustomizationEdit.vue'
import CustomizationList from './components/Customizations/CustomizationList.vue'
import Home from './components/Home.vue'
import NotFound from './components/NotFound.vue'
import Profile from './components/Identity/Profile.vue'
import RecoverPassword from './components/Identity/RecoverPassword.vue'
import ResetPassword from './components/Identity/ResetPassword.vue'
import SignIn from './components/Identity/SignIn.vue'
import SignUp from './components/Identity/SignUp.vue'
import WorldEdit from './components/Worlds/WorldEdit.vue'
import WorldList from './components/Worlds/WorldList.vue'

Vue.use(VueRouter)

const router = new VueRouter({
  mode: 'history',
  routes: [
    {
      name: 'Home',
      path: '/',
      component: Home,
      meta: { public: true }
    },
    {
      name: 'NotFound',
      path: '/404',
      component: NotFound,
      meta: { public: true }
    },
    // Identity
    {
      name: 'Confirm',
      path: '/user/confirm',
      component: Confirm,
      meta: { public: true }
    },
    {
      name: 'Profile',
      path: '/user/profile',
      component: Profile
    },
    {
      name: 'RecoverPassword',
      path: '/user/recover-password',
      component: RecoverPassword,
      meta: { public: true }
    },
    {
      name: 'ResetPassword',
      path: '/user/reset-password',
      component: ResetPassword,
      meta: { public: true }
    },
    {
      name: 'SignIn',
      path: '/user/sign-in',
      component: SignIn,
      meta: { public: true }
    },
    {
      name: 'SignUp',
      path: '/user/sign-up',
      component: SignUp,
      meta: { public: true }
    },
    // Aspects
    {
      name: 'AspectList',
      path: '/aspects',
      component: AspectList
    },
    // Customizations
    {
      name: 'CustomizationList',
      path: '/customizations',
      component: CustomizationList
    },
    {
      name: 'CustomizationEdit',
      path: '/customizations/:id',
      component: CustomizationEdit
    },
    // Worlds
    {
      name: 'WorldList',
      path: '/worlds',
      component: WorldList
    },
    {
      name: 'WorldEdit',
      path: '/worlds/:alias',
      component: WorldEdit
    },
    {
      name: 'CreateWorld',
      path: '/create-world',
      component: WorldEdit
    },
    {
      path: '*',
      redirect: '/404'
    }
  ]
})

router.beforeEach((to, _, next) => {
  if (!to.meta.public && !store.state.token) {
    return next({ name: 'SignIn' })
  }
  next()
})

export default router
