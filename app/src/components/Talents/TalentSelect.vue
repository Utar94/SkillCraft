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
    @input="onInput"
  />
</template>

<script>
import { getTalents } from '@/api/talents'

export default {
  data: () => ({
    talents: []
  }),
  props: {
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
      default: 'talent'
    },
    label: {
      type: String,
      default: 'talent.select.label'
    },
    placeholder: {
      type: String,
      default: 'talent.select.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    rules: {
      type: Object,
      default: null
    },
    tiers: {
      type: Array,
      default: () => [0, 1, 2, 3]
    },
    value: {}
  },
  computed: {
    options() {
      return this.talents
        .filter(({ id }) => !this.exclude.includes(id))
        .map(({ id, name }) => ({
          text: name,
          value: id
        }))
    }
  },
  methods: {
    onInput(id) {
      this.$emit('input', id)
      const talent = this.talents.find(talent => talent.id === id)
      if (talent) {
        this.$emit('talent', talent)
      }
    }
  },
  watch: {
    tiers: {
      immediate: true,
      async handler(newValue, oldValue) {
        if (JSON.stringify(newValue) !== JSON.stringify(oldValue)) {
          try {
            const { data } = await getTalents({
              deleted: false,
              tiers: newValue.join(','),
              sort: 'Name',
              desc: false
            })
            this.talents = data.items
          } catch (e) {
            this.handle(e)
          }
        }
      }
    }
  }
}
</script>
