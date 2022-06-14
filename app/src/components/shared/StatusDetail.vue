<template>
  <p>
    {{ $t('statusDetail.createdAt', { date: $d(createdAt, 'medium') }) }}
    <template v-if="deletedAt">
      <br />
      {{ $t('statusDetail.deletedAt', { date: $d(deletedAt, 'medium') }) }}
    </template>
    <template v-if="updatedAt">
      <br />
      {{ $t('statusDetail.updatedAt', { date: $d(updatedAt, 'medium') }) }}
    </template>
  </p>
</template>

<script>
export default {
  props: {
    createdAt: {
      type: Date,
      required: true
    },
    deletedAt: {
      type: Date,
      default: null
    },
    updatedAt: {
      type: Date,
      default: null
    }
  }
}
</script>
