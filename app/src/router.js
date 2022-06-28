import Vue from 'vue'
import VueRouter from 'vue-router'
import store from './store'
import AspectEdit from './components/Aspects/AspectEdit.vue'
import AspectList from './components/Aspects/AspectList.vue'
import CasteEdit from './components/Castes/CasteEdit.vue'
import CasteList from './components/Castes/CasteList.vue'
import ConditionEdit from './components/Conditions/ConditionEdit.vue'
import ConditionList from './components/Conditions/ConditionList.vue'
import Confirm from './components/Identity/Confirm.vue'
import CustomizationEdit from './components/Customizations/CustomizationEdit.vue'
import CustomizationList from './components/Customizations/CustomizationList.vue'
import EducationEdit from './components/Educations/EducationEdit.vue'
import EducationList from './components/Educations/EducationList.vue'
import Home from './components/Home.vue'
import LanguageEdit from './components/Languages/LanguageEdit.vue'
import LanguageList from './components/Languages/LanguageList.vue'
import NatureEdit from './components/Natures/NatureEdit.vue'
import NatureList from './components/Natures/NatureList.vue'
import NotFound from './components/NotFound.vue'
import PowerEdit from './components/Powers/PowerEdit.vue'
import PowerList from './components/Powers/PowerList.vue'
import Profile from './components/Identity/Profile.vue'
import RaceEdit from './components/Races/RaceEdit.vue'
import RaceList from './components/Races/RaceList.vue'
import RecoverPassword from './components/Identity/RecoverPassword.vue'
import ResetPassword from './components/Identity/ResetPassword.vue'
import SignIn from './components/Identity/SignIn.vue'
import SignUp from './components/Identity/SignUp.vue'
import TalentEdit from './components/Talents/TalentEdit.vue'
import TalentList from './components/Talents/TalentList.vue'
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
    {
      name: 'AspectEdit',
      path: '/aspects/:id',
      component: AspectEdit
    },
    // Castes
    {
      name: 'CasteList',
      path: '/castes',
      component: CasteList
    },
    {
      name: 'CasteEdit',
      path: '/castes/:id',
      component: CasteEdit
    },
    // Conditions
    {
      name: 'ConditionList',
      path: '/conditions',
      component: ConditionList
    },
    {
      name: 'ConditionEdit',
      path: '/conditions/:id',
      component: ConditionEdit
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
    // Educations
    {
      name: 'EducationList',
      path: '/educations',
      component: EducationList
    },
    {
      name: 'EducationEdit',
      path: '/educations/:id',
      component: EducationEdit
    },
    // Languages
    {
      name: 'LanguageList',
      path: '/languages',
      component: LanguageList
    },
    {
      name: 'LanguageEdit',
      path: '/languages/:id',
      component: LanguageEdit
    },
    // Natures
    {
      name: 'NatureList',
      path: '/natures',
      component: NatureList
    },
    {
      name: 'NatureEdit',
      path: '/natures/:id',
      component: NatureEdit
    },
    // Powers
    {
      name: 'PowerList',
      path: '/powers',
      component: PowerList
    },
    {
      name: 'PowerEdit',
      path: '/powers/:id',
      component: PowerEdit
    },
    // Races
    {
      name: 'RaceList',
      path: '/races',
      component: RaceList
    },
    {
      name: 'RaceEdit',
      path: '/races/:id',
      component: RaceEdit
    },
    {
      name: 'PeopleEdit',
      path: '/races/:raceId/people/:id',
      component: RaceEdit
    },
    // Talents
    {
      name: 'TalentList',
      path: '/talents',
      component: TalentList
    },
    {
      name: 'TalentEdit',
      path: '/talents/:id',
      component: TalentEdit
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
