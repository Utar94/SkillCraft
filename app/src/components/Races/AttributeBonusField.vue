<template>
  <form-field
    :disabled="disabled"
    :id="id || attribute.toLowerCase()"
    :label="label || `attribute.options.${attribute}`"
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
    attribute: {
      type: String,
      required: true
    },
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
      default: 10
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
