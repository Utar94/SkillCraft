import Vue from 'vue'
import Gravatar from 'vue-gravatar'
import AttributeSelect from './components/shared/AttributeSelect.vue'
import CountSelect from './components/shared/CountSelect.vue'
import DeleteModal from './components/shared/DeleteModal.vue'
import DescriptionField from './components/shared/DescriptionField.vue'
import FormDateTime from './components/shared/FormDateTime.vue'
import FormField from './components/shared/FormField.vue'
import FormSelect from './components/shared/FormSelect.vue'
import FormTextarea from './components/shared/FormTextarea.vue'
import IconButton from './components/shared/IconButton.vue'
import IconSubmit from './components/shared/IconSubmit.vue'
import NameField from './components/shared/NameField.vue'
import PasswordField from './components/shared/PasswordField.vue'
import SearchField from './components/shared/SearchField.vue'
import SizeCategorySelect from './components/shared/SizeCategorySelect.vue'
import SkillSelect from './components/shared/SkillSelect.vue'
import SortSelect from './components/shared/SortSelect.vue'
import StatusDetail from './components/shared/StatusDetail.vue'
import TagList from './components/shared/TagList.vue'
import TierSelect from './components/shared/TierSelect.vue'

Vue.component('v-gravatar', Gravatar)

Vue.component('attribute-select', AttributeSelect)
Vue.component('count-select', CountSelect)
Vue.component('delete-modal', DeleteModal)
Vue.component('description-field', DescriptionField)
Vue.component('form-datetime', FormDateTime)
Vue.component('form-field', FormField)
Vue.component('form-select', FormSelect)
Vue.component('form-textarea', FormTextarea)
Vue.component('icon-button', IconButton)
Vue.component('icon-submit', IconSubmit)
Vue.component('name-field', NameField)
Vue.component('password-field', PasswordField)
Vue.component('search-field', SearchField)
Vue.component('size-category-select', SizeCategorySelect)
Vue.component('skill-select', SkillSelect)
Vue.component('sort-select', SortSelect)
Vue.component('status-detail', StatusDetail)
Vue.component('tag-list', TagList)
Vue.component('tier-select', TierSelect)

Vue.mixin({
  methods: {
    getValidationState({ dirty, validated, valid = null }) {
      return dirty || validated ? valid : null
    },
    handleError(e = null) {
      if (e) {
        console.error(e)
      }
      this.toast('errorToast.title', 'errorToast.body', 'danger')
    },
    orderBy(items, key = null) {
      return key ? [...items].sort((a, b) => (a[key] < b[key] ? -1 : a[key] > b[key] ? 1 : 0)) : [...items].sort((a, b) => (a < b ? -1 : a > b ? 1 : 0))
    },
    roll(roll) {
      let result = 0
      const parts = roll.split('+')
      const dice = parts[0].split('d')
      for (let i = 0; i < Number(dice[0]); i++) {
        result += Math.floor(Math.random() * Number(dice[1])) + 1
      }
      if (parts.length > 1) {
        result += Number(parts[1])
      }
      return result
    },
    shortify(text, length) {
      return text?.length > length ? text.substring(0, length - 1) + '…' : text
    },
    toast(title, body = '', variant = 'success') {
      this.$bvToast.toast(this.$i18n.t(body), {
        solid: true,
        title: this.$i18n.t(title),
        variant
      })
    }
  }
})
