<template>
  <b-container>
    <h1 v-t="'worlds.title'" />
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" icon="plus" text="actions.create" :to="{ name: 'CreateWorld' }" variant="success" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @descInput="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="items.length">
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
              <router-link :to="{ name: 'WorldEdit', params: { alias: item.alias } }" v-text="item.name" />
            </td>
            <td v-text="item.alias" />
            <td>{{ $d(new Date(item.updatedAt || item.createdAt), 'medium') }}</td>
            <td>
              <b-badge v-if="world && world.id === item.id" class="mx-1" variant="primary">
                <font-awesome-icon icon="star" />
                {{ $t('worlds.current') }}
              </b-badge>
              <icon-button v-else class="mx-1" icon="globe" text="worlds.select" variant="primary" @click="changeWorld(item)" />
              <icon-button class="mx-1" icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`deleteWorld_${item.id}`" />
              <delete-modal
                confirm="worlds.delete.confirm"
                :disabled="loading"
                :displayName="item.name"
                :id="`deleteWorld_${item.id}`"
                :loading="loading"
                title="worlds.delete.title"
                @ok="_delete(item, $event)"
              >
                <p><strong v-t="'worlds.delete.warning'" /></p>
              </delete-modal>
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination :per-page="count" :total-rows="total" v-model="page" />
    </template>
    <p v-else v-t="'noResult'" />
  </b-container>
</template>

<script>
import { mapActions, mapState } from 'vuex'
import { deleteWorld, getWorlds } from '@/api/worlds'

export default {
  data: () => ({
    count: 10,
    desc: false,
    items: [],
    loading: false,
    page: 1,
    search: null,
    sort: 'Name',
    total: 0
  }),
  computed: {
    ...mapState(['world']),
    params() {
      return {
        search: this.search,
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    },
    sortOptions() {
      return Object.entries(this.$i18n.t('worlds.sort'))
        .map(([value, text]) => ({ text, value }))
        .sort((a, b) => (a < b ? -1 : a > b ? 1 : 0))
    }
  },
  methods: {
    ...mapActions(['changeWorld']),
    async _delete({ id }, callback = null) {
      if (!this.loading) {
        this.loading = true
        let refresh = false
        try {
          await deleteWorld(id)
          refresh = true
          this.toast('success', 'worlds.delete.success')
          if (typeof callback === 'function') {
            callback()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
        if (refresh) {
          await this.refresh()
        }
      }
    },
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
      async handler(newValue, oldValue) {
        if (newValue?.index && oldValue && (newValue.search !== oldValue.search || newValue.sort !== oldValue.sort || newValue.count !== oldValue.count)) {
          this.page = 1
          await this.refresh()
        } else {
          await this.refresh(newValue)
        }
      }
    }
  }
}
</script>
