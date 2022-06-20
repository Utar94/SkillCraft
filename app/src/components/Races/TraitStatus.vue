<template>
  <b-badge :variant="variant">
    {{ $t(`race.traits.status.${status}`) }}
  </b-badge>
</template>

<script>
export default {
  props: {
    status: {
      type: String,
      required: true
    }
  },
  data: () => ({
    variant: null
  }),
  watch: {
    status: {
      immediate: true,
      handler(status) {
        switch (status) {
          case 'added':
            this.variant = 'success'
            break
          case 'removed':
            this.variant = 'danger'
            break
          case 'updated':
            this.variant = 'info'
            break
        }
      }
    }
  }
}
</script>
