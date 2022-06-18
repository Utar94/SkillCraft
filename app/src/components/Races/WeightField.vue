<template>
  <form-field
    :disabled="disabled"
    :id="id"
    :label="label"
    :placeholder="placeholder"
    :required="required"
    :rules="rules"
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
      required: true
    },
    label: {
      type: String,
      required: true
    },
    placeholder: {
      type: String,
      default: 'race.physical.weight.roll'
    },
    required: {
      type: Boolean,
      default: false
    },
    rules: {
      type: Object,
      default: () => ({ regex: /^\d{1,2}d\d{1,2}(\+\d{1,3})?$/ })
    },
    step: {
      type: Number
    },
    type: {
      type: String,
      default: 'text'
    },
    value: {}
  }
}
</script>
