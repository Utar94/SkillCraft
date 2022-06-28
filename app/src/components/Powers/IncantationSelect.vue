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
  >
    <b-form-checkbox id="ritual" :checked="ritual" @input="$emit('ritualInput', $event)">{{ $t('power.incantation.ritual') }}</b-form-checkbox>
  </form-select>
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
      default: 'incantation'
    },
    label: {
      type: String,
      default: 'power.incantation.label'
    },
    placeholder: {
      type: String,
      default: 'power.incantation.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    ritual: {
      type: Boolean,
      required: true
    },
    rules: {
      type: Object,
      default: null
    },
    value: {}
  },
  computed: {
    options() {
      return Object.entries(this.$i18n.t('power.incantation.options')).map(([value, text]) => ({ text, value }))
    }
  }
}
</script>
