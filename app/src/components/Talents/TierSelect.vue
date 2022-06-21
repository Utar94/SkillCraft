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
      default: 'tier'
    },
    label: {
      type: String,
      default: 'talent.tier.label'
    },
    options: {
      type: Array,
      default: () => [0, 1, 2, 3]
    },
    placeholder: {
      type: String,
      default: 'talent.tier.placeholder'
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
  }
}
</script>
