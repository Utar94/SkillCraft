<template>
  <form-select
    :disabled="disabled"
    :id="id"
    :label="label"
    :options="options"
    :placeholder="placeholder"
    :required="disabled ? false : required"
    :rules="disabled ? null : rules"
    :value="value ? value.id : null"
    @input="select($event)"
  />
</template>

<script>
export default {
  props: {
    aspects: {
      type: Array,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    exclude: {
      type: Array,
      default: () => []
    },
    id: {
      type: String,
      default: 'aspect'
    },
    label: {
      type: String,
      default: ''
    },
    placeholder: {
      type: String,
      default: 'character.selectAspect'
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
  },
  computed: {
    options() {
      return this.aspects
        .filter(({ id }) => !this.exclude.includes(id))
        .map(({ id, name }) => ({
          text: name,
          value: id
        }))
    }
  },
  methods: {
    select(id) {
      const aspect = this.aspects.find(aspect => aspect.id === id)
      if (aspect) {
        this.$emit('input', aspect)
      }
    }
  }
}
</script>
