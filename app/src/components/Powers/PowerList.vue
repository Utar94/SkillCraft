<template>
  <b-container>
    <h1 v-t="'powers.title'" />
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" icon="plus" text="actions.create" :to="{ name: 'PowerEdit', params: { id: 'new' } }" variant="success" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <tier-select class="col" v-model="tier" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @descInput="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="items.length">
      <table class="table table-striped">
        <thead>
          <tr>
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'tier.label'" />
            <th scope="col" v-t="'power.incantation.label'" />
            <th scope="col" v-t="'updatedAt'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in items" :key="item.id">
            <td>
              <router-link :to="{ name: 'PowerEdit', params: { id: item.id } }" v-text="item.name" />
            </td>
            <td v-text="item.tier" />
            <td>{{ $t(`power.incantation.options.${item.incantation}`) }}</td>
            <td>{{ $d(new Date(item.updatedAt || item.createdAt), 'medium') }}</td>
            <td>
              <icon-button class="mx-1" icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`deletePower_${item.id}`" />
              <delete-modal
                confirm="powers.delete.confirm"
                :disabled="loading"
                :displayName="item.name"
                :id="`deletePower_${item.id}`"
                :loading="loading"
                title="powers.delete.title"
                @ok="_delete(item, $event)"
              />
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
import { deletePower, getPowers } from '@/api/powers'

export default {
  data: () => ({
    count: 10,
    deleted: false,
    desc: false,
    items: [],
    loading: false,
    page: 1,
    search: null,
    sort: 'Name',
    tier: null,
    total: 0
  }),
  computed: {
    params() {
      return {
        deleted: this.deleted,
        search: this.search,
        tiers: this.tier === null ? null : this.tier.toString(),
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    },
    sortOptions() {
      return Object.entries(this.$i18n.t('powers.sort'))
        .map(([value, text]) => ({ text, value }))
        .sort((a, b) => (a < b ? -1 : a > b ? 1 : 0))
    }
  },
  methods: {
    async _delete({ id }, callback = null) {
      if (!this.loading) {
        this.loading = true
        let refresh = false
        try {
          await deletePower(id)
          refresh = true
          this.toast('success', 'powers.delete.success')
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
          const { data } = await getPowers(params ?? this.params)
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
        if (
          newValue?.index &&
          oldValue &&
          (newValue.deleted !== oldValue.deleted ||
            newValue.search !== oldValue.search ||
            newValue.tier !== oldValue.tier ||
            newValue.sort !== oldValue.sort ||
            newValue.count !== oldValue.count)
        ) {
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
