<template>
  <form-textarea
    :disabled="disabled"
    :id="id"
    :label="label"
    :maxLength="maxLength"
    :minLength="minLength"
    :placeholder="placeholder"
    :required="required"
    :rows="rows"
    :rules="rules"
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
      default: 'description'
    },
    label: {
      type: String,
      default: 'description.label'
    },
    maxLength: {
      type: Number
    },
    minLength: {
      type: Number
    },
    placeholder: {
      type: String,
      default: 'description.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    rows: {
      type: Number,
      default: 15
    },
    rules: {
      type: Object,
      default: null
    },
    value: {}
  }
}
</script>
