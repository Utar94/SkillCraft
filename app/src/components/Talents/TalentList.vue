<template>
  <b-container>
    <h1 v-t="'talents.title'" />
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" icon="plus" text="actions.create" :to="{ name: 'TalentEdit', params: { id: 'new' } }" variant="success" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <form-select
        class="col"
        id="multipleAcquisition"
        label="talents.multipleAcquisition.label"
        :options="multipleAcquisitionOptions"
        placeholder="talents.multipleAcquisition.placeholder"
        v-model="multipleAcquisition"
      />
      <tier-select class="col" v-model="tier" />
    </b-row>
    <b-row>
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @descInput="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="items.length">
      <table class="table table-striped">
        <thead>
          <tr>
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'talent.tier.label'" />
            <th scope="col" v-t="'talent.required.label'" />
            <th scope="col" v-t="'updatedAt'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in items" :key="item.id">
            <td>
              <router-link :to="{ name: 'TalentEdit', params: { id: item.id } }" v-text="item.name" />
            </td>
            <td v-text="item.tier" />
            <td>
              <router-link v-if="item.requiredTalent" :to="{ name: 'TalentEdit', params: { id: item.requiredTalent.id } }" v-text="item.requiredTalent.name" />
            </td>
            <td>{{ $d(new Date(item.updatedAt || item.createdAt), 'medium') }}</td>
            <td>
              <icon-button class="mx-1" icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`deleteTalent_${item.id}`" />
              <delete-modal
                confirm="talents.delete.confirm"
                :disabled="loading"
                :displayName="item.name"
                :id="`deleteTalent_${item.id}`"
                :loading="loading"
                title="talents.delete.title"
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
import TierSelect from './TierSelect.vue'
import { deleteTalent, getTalents } from '@/api/talents'

export default {
  components: {
    TierSelect
  },
  data: () => ({
    count: 10,
    deleted: false,
    desc: false,
    items: [],
    loading: false,
    multipleAcquisition: null,
    page: 1,
    search: null,
    sort: 'Name',
    tier: null,
    total: 0
  }),
  computed: {
    multipleAcquisitionOptions() {
      return Object.entries(this.$i18n.t('talents.multipleAcquisition.options')).map(([value, text]) => ({ text, value }))
    },
    params() {
      return {
        deleted: this.deleted,
        multipleAcquisition: this.multipleAcquisition === 'yes' ? true : this.multipleAcquisition === 'no' ? false : null,
        search: this.search,
        tiers: this.tier === null ? null : this.tier.toString(),
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    },
    sortOptions() {
      return Object.entries(this.$i18n.t('talents.sort'))
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
          await deleteTalent(id)
          refresh = true
          this.toast('success', 'talents.delete.success')
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
          const { data } = await getTalents(params ?? this.params)
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
            newValue.multipleAcquisition !== oldValue.multipleAcquisition ||
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
