<template>
  <b-container>
    <template v-if="world">
      <h1 v-text="title" />
      <status-detail :createdAt="new Date(world.createdAt)" :updatedAt="world.updatedAt ? new Date(world.updatedAt) : null" />
      <p>{{ $t('world.alias.format', { alias: world.alias }) }}</p>
      <validation-observer ref="form">
        <b-form @submit.prevent="submit">
          <name-field v-model="name" />
          <description-field v-model="description" />
          <div class="my-2">
            <icon-submit class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
            <icon-button class="mx-1" icon="ban" text="actions.cancel" :to="{ name: 'WorldList' }" />
          </div>
        </b-form>
      </validation-observer>
    </template>
  </b-container>
</template>

<script>
import { getWorld, updateWorld } from '@/api/worlds'

export default {
  data: () => ({
    alias: null,
    description: null,
    loading: false,
    name: null,
    world: null
  }),
  computed: {
    hasChanges() {
      return this.name !== this.world.name || (this.description ?? '') !== (this.world.description ?? '')
    },
    title() {
      return this.world.name
    }
  },
  methods: {
    setModel(model) {
      this.world = model
      this.description = model.description
      this.name = model.name
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await updateWorld(this.world.id, {
              description: this.description,
              name: this.name
            })
            this.setModel(data)
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  async created() {
    try {
      const { data } = await getWorld(this.$route.params.id)
      this.setModel(data)
    } catch (e) {
      if (e.status === 404) {
        return this.$router.push({ name: 'NotFound' })
      }
      this.handleError(e)
    }
  }
}
</script>
