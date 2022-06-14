<template>
  <b-container>
    <h1 v-t="'aspects.title'" />
    <icon-button :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
  </b-container>
</template>

<script>
import { getAspects } from '@/api/aspects'

export default {
  data: () => ({
    count: 10,
    deleted: false,
    desc: false,
    items: [],
    loading: false,
    page: 1,
    search: null,
    sort: null,
    total: 0
  }),
  computed: {
    params() {
      return {
        deleted: this.deleted,
        search: this.search,
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    }
  },
  methods: {
    async refresh(params = null) {
      if (!this.loading) {
        this.loading = true
        try {
          const { data } = await getAspects(params ?? this.params)
          this.items = data.items
          this.total = data.total
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  watch: {
    params: {
      deep: true,
      immediate: true,
      async handler(params) {
        await this.refresh(params)
      }
    }
  }
}
</script>
