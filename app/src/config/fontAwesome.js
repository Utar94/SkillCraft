import Vue from 'vue'
import { library } from '@fortawesome/fontawesome-svg-core'
import {
  faArrowLeft,
  faBan,
  faCog,
  faEdit,
  faEye,
  faGlobe,
  faGraduationCap,
  faHome,
  faKey,
  faLanguage,
  faPaperPlane,
  faPaw,
  faPlus,
  faSave,
  faSearch,
  faSignInAlt,
  faSignOutAlt,
  faStar,
  faSyncAlt,
  faTasks,
  faTrashAlt,
  faUser
} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(
  faArrowLeft,
  faBan,
  faCog,
  faEdit,
  faEye,
  faGlobe,
  faGraduationCap,
  faHome,
  faKey,
  faLanguage,
  faPaperPlane,
  faPaw,
  faPlus,
  faSave,
  faSearch,
  faSignInAlt,
  faSignOutAlt,
  faStar,
  faSyncAlt,
  faTasks,
  faTrashAlt,
  faUser
)

Vue.component('font-awesome-icon', FontAwesomeIcon)
