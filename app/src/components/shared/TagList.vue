<template>
  <b-form-group :label="$t(label)">
    <div class="form-control">
      <template v-if="tags.length">
        <b-badge v-for="tag in tags" :key="tag.value" class="mx-1" :variant="tag.variant">
          {{ tag.text }}
          <strong v-if="!disabled && !tag.disabled" class="remove" @click="$emit('remove', tag)">&times;</strong>
        </b-badge>
      </template>
      <span v-else class="empty">{{ $t(placeholder) }}</span>
    </div>
  </b-form-group>
</template>

<script>
export default {
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    label: {
      type: String,
      default: 'tags.label'
    },
    placeholder: {
      type: String,
      default: 'tags.placeholder'
    },
    tags: {
      type: Array,
      required: true
    }
  }
}
</script>

<style scoped>
.empty {
  color: #6c757d;
}

.remove {
  cursor: pointer;
}
</style>
