<template>
  <b-container>
    <h1 v-t="'worlds.title'" />
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" icon="plus" text="actions.create" variant="success" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <!-- Sort -->
      <!-- Count -->
    </b-row>
    <table class="table table-striped">
      <thead>
        <tr>
          <th scope="col" v-t="'name.label'" />
          <th scope="col" v-t="'world.alias.label'" />
          <th scope="col" v-t="'updatedAt'" />
          <th scope="col" />
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.id">
          <td>
            <router-link :to="{ name: 'WorldEdit', params: { id: item.id } }" v-text="item.name" />
          </td>
          <td v-text="item.alias" />
          <td>{{ $d(new Date(item.updatedAt || item.createdAt), 'medium') }}</td>
          <td>
            <icon-button disabled icon="trash-alt" text="actions.delete" variant="danger" />
          </td>
        </tr>
      </tbody>
    </table>
    <!-- Pager -->
    <!-- No result -->
  </b-container>
</template>

<script>
import { getWorlds } from '@/api/worlds'

export default {
  data: () => ({
    count: 10,
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
          const { data } = await getWorlds(params ?? this.params)
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
