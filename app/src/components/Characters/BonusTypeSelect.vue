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
    id: {
      type: String,
      default: 'type'
    },
    label: {
      type: String,
      default: 'character.bonuses.types.label'
    },
    placeholder: {
      type: String,
      default: 'character.bonuses.types.placeholder'
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
      return [
        { text: this.$i18n.t('character.bonuses.types.Attribute'), value: 'Attribute' },
        { text: this.$i18n.t('character.bonuses.types.Statistic'), value: 'Statistic' },
        { text: this.$i18n.t('character.bonuses.types.Skill'), value: 'Skill' },
        { text: this.$i18n.t('character.bonuses.types.Other'), value: 'Other' }
      ]
    }
  }
}
</script>
