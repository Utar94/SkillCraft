<template>
  <b-container>
    <h1 v-text="title" />
    <b-alert dismissible variant="warning" v-model="aliasConflict">
      <strong v-t="'world.creationFailed'" />
      {{ $t('world.alias.conflict') }}
    </b-alert>
    <status-detail v-if="world" :createdAt="new Date(world.createdAt)" :updatedAt="world.updatedAt ? new Date(world.updatedAt) : null" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <name-field v-model="name" />
        <alias-field :disabled="Boolean(world)" :name="name" v-model="alias" />
        <description-field v-model="description" />
        <div class="my-2">
          <icon-submit v-if="world" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="ban" text="actions.cancel" :to="{ name: 'WorldList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import AliasField from './AliasField.vue'
import { createWorld, getWorld, updateWorld } from '@/api/worlds'

export default {
  components: {
    AliasField
  },
  data: () => ({
    alias: null,
    aliasConflict: false,
    description: null,
    loading: false,
    name: null,
    world: null
  }),
  computed: {
    hasChanges() {
      return this.name !== this.world?.name || (this.description ?? '') !== (this.world?.description ?? '')
    },
    title() {
      return this.world?.name ?? this.$i18n.t('world.title')
    }
  },
  methods: {
    setModel(model) {
      this.world = model
      this.alias = model.alias
      this.description = model.description
      this.name = model.name
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.aliasConflict = false
        try {
          if (await this.$refs.form.validate()) {
            if (this.world) {
              const { data } = await updateWorld(this.world.id, {
                description: this.description,
                name: this.name
              })
              this.setModel(data)
              this.toast('success', 'world.updated')
            } else {
              const { data } = await createWorld({
                alias: this.alias,
                description: this.description,
                name: this.name
              })
              this.setModel(data)
              this.toast('success', 'world.created')
            }
          }
        } catch (e) {
          if (e.status === 409 && e.data?.field === 'Alias') {
            this.aliasConflict = true
          } else {
            this.handleError(e)
          }
        } finally {
          this.loading = false
        }
      }
    }
  },
  async created() {
    const alias = this.$route.params.alias
    if (alias) {
      try {
        const { data } = await getWorld(alias)
        this.setModel(data)
      } catch (e) {
        if (e.status === 404) {
          return this.$router.push({ name: 'NotFound' })
        }
        this.handleError(e)
      }
    }
  }
}
</script>
