<template>
  <form-field
    :disabled="disabled"
    :id="id || speedType.toLowerCase()"
    :label="label || `speed.types.${speedType}`"
    :maxValue="disabled ? null : maxValue"
    :minValue="disabled ? null : minValue"
    :required="disabled ? false : required"
    :rules="disabled ? null : rules"
    :step="step"
    :type="type"
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
      default: ''
    },
    label: {
      type: String,
      default: ''
    },
    maxValue: {
      type: Number,
      default: 99
    },
    minValue: {
      type: Number,
      default: 0
    },
    required: {
      type: Boolean,
      default: false
    },
    rules: {
      type: Object,
      default: null
    },
    speedType: {
      type: String,
      required: true
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
  }
}
</script>
