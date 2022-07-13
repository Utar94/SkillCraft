<template>
  <form-select
    :disabled="disabled"
    :id="id"
    :label="label"
    :options="options"
    :placeholder="placeholder"
    :required="disabled ? false : required"
    :rules="disabled ? null : rules"
    :value="value"
    @input="$emit('input', $event)"
  />
</template>

<script>
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
      default: 'otherBonus'
    },
    label: {
      type: String,
      default: 'character.bonuses.other.label'
    },
    placeholder: {
      type: String,
      default: 'character.bonuses.other.placeholder'
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
  computed: {
    options() {
      return Object.entries(this.$i18n.t('character.bonuses.other.options'))
        .filter(([value]) => !this.exclude.includes(value))
        .map(([value, text]) => ({ text, value }))
        .sort((a, b) => (a < b ? -1 : a > b ? 1 : 0))
    }
  }
}
</script>
