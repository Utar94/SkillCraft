<template>
  <form-field
    :disabled="disabled || !ranged"
    :id="id"
    :label="label"
    :maxValue="disabled || !ranged ? null : maxValue"
    :minValue="disabled || !ranged ? null : minValue"
    :required="disabled || !ranged ? false : required"
    :rules="disabled ? null : rules"
    :step="step"
    :type="type"
    :value="value"
    @input="$emit('input', $event)"
  >
    <b-form-checkbox id="self" :checked="self" @change="$emit('input', self ? 1 : null)">
      {{ $t('power.range.self') }}
    </b-form-checkbox>
    <b-form-checkbox id="touch" :checked="touch" @change="$emit('input', touch ? 1 : 0)">
      {{ $t('power.range.touch') }}
    </b-form-checkbox>
  </form-field>
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
      default: 'range'
    },
    label: {
      type: String,
      default: 'power.range.label'
    },
    maxValue: {
      type: Number,
      default: 1056
    },
    minValue: {
      type: Number,
      default: 1
    },
    required: {
      type: Boolean,
      default: false
    },
    rules: {
      type: Object,
      default: () => null
    },
    step: {
      type: Number,
      default: 1
    },
    type: {
      type: String,
      default: 'number'
    },
    value: {}
  },
  computed: {
    ranged() {
      return this.value !== null && Number(this.value) > 0
    },
    self() {
      return this.value === null
    },
    touch() {
      return this.value !== null && Number(this.value) === 0
    }
  }
}
</script>
