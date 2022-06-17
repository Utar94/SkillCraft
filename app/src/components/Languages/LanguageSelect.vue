<template>
  <form-select
    :disabled="disabled"
    :id="id"
    :label="label"
    :options="languageOptions"
    :placeholder="placeholder"
    :required="disabled ? false : required"
    :rules="disabled ? null : rules"
    :value="value"
    @input="$emit('input', $event)"
  />
</template>

<script>
import { getLanguages } from '@/api/languages'

export default {
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    exclude: {
      type: Array,
      default: () => []
    },
    id: {
      type: String,
      default: 'language'
    },
    label: {
      type: String,
      default: 'language.select.label'
    },
    options: {
      type: Array
    },
    placeholder: {
      type: String,
      default: 'language.select.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    rules: {
      type: Object,
      default: null
    },
    value: {}
  },
  data: () => ({
    languages: []
  }),
  computed: {
    languageOptions() {
      return (
        this.options ??
        this.languages
          .filter(({ id }) => !this.exclude.includes(id))
          .map(({ id, name }) => ({
            text: name,
            value: id
          }))
      )
    }
  },
  async created() {
    if (!this.options) {
      try {
        const { data } = await getLanguages({ deleted: false, sort: 'Name', desc: false })
        this.languages = data.items
      } catch (e) {
        this.handleError(e)
      }
    }
  }
}
</script>
