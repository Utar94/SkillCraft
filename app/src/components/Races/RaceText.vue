<template>
  <form-textarea
    :disabled="disabled"
    :id="id"
    :label="label"
    :maxLength="disabled ? null : maxLength"
    :minLength="disabled ? null : minLength"
    :placeholder="placeholder"
    :required="disabled ? false : required"
    :rows="rows"
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
      type: String
    },
    label: {
      type: String,
      default: 'description.label'
    },
    maxLength: {
      type: Number,
      default: 1000
    },
    minLength: {
      type: Number,
      default: 0
    },
    placeholder: {
      type: String,
      default: ''
    },
    required: {
      type: Boolean,
      default: false
    },
    rows: {
      type: Number,
      default: 5
    },
    rules: {
      type: Object,
      default: null
    },
    value: {}
  }
}
</script>
