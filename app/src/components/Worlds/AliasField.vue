<template>
  <form-field
    :disabled="!custom || disabled"
    :id="id"
    :label="label"
    :maxLength="disabled ? null : maxLength"
    :minLength="disabled ? null : minLength"
    :placeholder="placeholder"
    :required="disabled ? false : required"
    :rules="disabled ? null : rules"
    :value="value"
    @input="$emit('input', $event)"
  >
    <b-form-checkbox :disabled="disabled" v-model="custom">{{ $t('world.alias.custom') }}</b-form-checkbox>
  </form-field>
</template>

<script>
import { isLetterOrDigit } from '@/helpers/stringUtils'

export default {
  data: () => ({
    custom: false
  }),
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'alias'
    },
    label: {
      type: String,
      default: 'world.alias.label'
    },
    maxLength: {
      type: Number,
      default: 100
    },
    minLength: {
      type: Number,
      default: 0
    },
    name: {
      type: String,
      default: ''
    },
    placeholder: {
      type: String,
      default: 'world.alias.placeholder'
    },
    required: {
      type: Boolean,
      default: true
    },
    rules: {
      type: Object,
      default: () => ({ alias: true })
    },
    value: {}
  },
  methods: {
    aliasify(name) {
      name ??= ''
      const words = []
      let word = ''
      for (const c of name) {
        if (isLetterOrDigit(c)) {
          word += c
        } else if (word.length) {
          words.push(word)
          word = ''
        }
      }
      if (word.length) {
        words.push(word)
      }
      return words.join('-').toLowerCase()
    }
  },
  watch: {
    custom(newValue, oldValue) {
      if (newValue !== oldValue && !newValue) {
        this.$emit('input', this.aliasify(this.name))
      }
    },
    name: {
      immediate: true,
      handler(name) {
        if (!this.custom) {
          this.$emit('input', this.aliasify(name))
        }
      }
    }
  }
}
</script>
