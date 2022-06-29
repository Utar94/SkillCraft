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
    <b-form-checkbox id="concentration" :checked="concentration" :disabled="instantaneous" @input="$emit('concentrationInput', $event)">{{
      $t('power.duration.concentration')
    }}</b-form-checkbox>
  </form-select>
</template>

<script>
export default {
  props: {
    concentration: {
      type: Boolean,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'duration'
    },
    label: {
      type: String,
      default: 'power.duration.label'
    },
    placeholder: {
      type: String,
      default: 'power.duration.placeholder'
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
    instantaneous() {
      return this.value !== null && Number(this.value) === 0
    },
    options() {
      return Object.entries(this.$i18n.t('power.duration.options')).map(([value, text]) => ({ text, value }))
    }
  },
  watch: {
    value: {
      immediate: true,
      handler(newValue, oldValue) {
        if (newValue !== oldValue && newValue === '0') {
          this.$emit('concentrationInput', false)
        }
      }
    }
  }
}
</script>
