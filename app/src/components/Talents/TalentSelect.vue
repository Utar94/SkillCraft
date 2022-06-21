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
      default: 'talent.required.label'
    },
    maxTier: {
      type: Number,
      default: 3
    },
    placeholder: {
      type: String,
      default: 'talent.required.placeholder'
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
      return this.talents
        .filter(({ id }) => !this.exclude.includes(id))
        .map(({ id, name }) => ({
          text: name,
          value: id
        }))
    }
  },
  watch: {
    maxTier: {
      immediate: true,
      async handler(maxTier) {
        try {
          const { data } = await getTalents({
            deleted: false,
            tiers: Array.from(new Array(maxTier + 1), (_, i) => i).join(','),
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
</script>
