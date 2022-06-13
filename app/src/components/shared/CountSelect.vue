<template>
  <form-select :disabled="disabled" :id="id" :label="label" :options="options" :value="value" @input="$emit('input', $event)" />
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
      default: 'count'
    },
    label: {
      type: String,
      default: 'count'
    },
    options: {
      type: Array,
      default: () => [10, 25, 50, 100]
    },
    value: {}
  }
}
</script>
