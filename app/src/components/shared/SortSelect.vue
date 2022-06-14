<template>
  <form-select :disabled="disabled" :id="id" :label="label" :options="options" :placeholder="placeholder" :value="value" @input="$emit('input', $event)">
    <b-form-checkbox id="desc" :checked="desc" @input="$emit('descInput', $event)">{{ $t('sort.desc') }}</b-form-checkbox>
  </form-select>
</template>

<script>
export default {
  props: {
    desc: {
      type: Boolean,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'sort'
    },
    label: {
      type: String,
      default: 'sort.label'
    },
    options: {
      type: Array,
      required: true
    },
    placeholder: {
      type: String,
      default: 'sort.placeholder'
    },
    value: {}
  }
}
</script>
