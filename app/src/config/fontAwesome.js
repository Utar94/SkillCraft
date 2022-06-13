import Vue from 'vue'
import { library } from '@fortawesome/fontawesome-svg-core'
import {
  faBan,
  faCog,
  faEdit,
  faEye,
  faGlobe,
  faHome,
  faKey,
  faPaperPlane,
  faPlus,
  faSave,
  faSearch,
  faSignInAlt,
  faSignOutAlt,
  faSyncAlt,
  faTasks,
  faTrashAlt,
  faUser
} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(
  faBan,
  faCog,
  faEdit,
  faEye,
  faGlobe,
  faHome,
  faKey,
  faPaperPlane,
  faPlus,
  faSave,
  faSearch,
  faSignInAlt,
  faSignOutAlt,
  faSyncAlt,
  faTasks,
  faTrashAlt,
  faUser
)

Vue.component('font-awesome-icon', FontAwesomeIcon)
