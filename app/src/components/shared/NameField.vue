<template>
  <form-field
    :disabled="disabled"
    :id="id"
    :label="label"
    :maxLength="maxLength"
    :minLength="minLength"
    :placeholder="placeholder"
    :required="required"
    :rules="rules"
    :value="value"
    @input="$emit('input', $event)"
  >
    <template #prepend>
      <slot name="prepend" />
    </template>
    <template #append>
      <slot name="append" />
    </template>
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
      default: 'name'
    },
    label: {
      type: String,
      default: 'name.label'
    },
    maxLength: {
      type: Number,
      default: 100
    },
    minLength: {
      type: Number,
      default: 0
    },
    placeholder: {
      type: String,
      default: 'name.placeholder'
    },
    required: {
      type: Boolean,
      default: true
    },
    rules: {
      type: Object,
      default: null
    },
    value: {}
  }
}
</script>
