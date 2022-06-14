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
import { getCustomizations } from '@/api/customizations'

export default {
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'feat'
    },
    label: {
      type: String,
      default: 'nature.feat.label'
    },
    placeholder: {
      type: String,
      default: 'nature.feat.placeholder'
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
  data: () => ({
    feats: []
  }),
  computed: {
    options() {
      return this.feats.map(({ id, name }) => ({
        text: name,
        value: id
      }))
    }
  },
  async created() {
    try {
      const { data } = await getCustomizations({
        deleted: false,
        type: 'Feat',
        sort: 'Name',
        desc: false
      })
      this.feats = data.items
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
