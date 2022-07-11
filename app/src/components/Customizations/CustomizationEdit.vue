<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="customization"
      :createdAt="new Date(customization.createdAt)"
      :deletedAt="customization.deletedAt ? new Date(customization.deletedAt) : null"
      :updatedAt="customization.updatedAt ? new Date(customization.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <customization-type-select :disabled="Boolean(customization)" required v-model="type" />
        <name-field v-model="name" />
        <description-field v-model="description" />
        <div class="my-2">
          <icon-submit
            v-if="customization"
            class="mx-1"
            :disabled="!hasChanges || loading"
            icon="save"
            :loading="loading"
            text="actions.save"
            variant="primary"
          />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'CustomizationList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import CustomizationTypeSelect from './CustomizationTypeSelect.vue'
import { createCustomization, getCustomization, updateCustomization } from '@/api/customizations'

export default {
  components: {
    CustomizationTypeSelect
  },
  data: () => ({
    customization: null,
    description: null,
    loading: false,
    name: null,
    type: null
  }),
  computed: {
    hasChanges() {
      return (this.name ?? '') !== (this.customization?.name ?? '') || (this.description ?? '') !== (this.customization?.description ?? '')
    },
    payload() {
      const payload = {
        description: this.description,
        name: this.name
      }
      if (!this.customization) {
        payload.type = this.type
      }
      return payload
    },
    title() {
      return this.customization?.name ?? this.$i18n.t('customization.title')
    }
  },
  methods: {
    setModel(model) {
      this.customization = model
      this.description = model.description
      this.name = model.name
      this.type = model.type
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.customization) {
              const { data } = await updateCustomization(this.customization.id, this.payload)
              this.setModel(data)
              this.toast('success', 'customization.updated')
            } else {
              const { data } = await createCustomization(this.payload)
              this.setModel(data)
              this.toast('success', 'customization.created')
            }
            this.$refs.form.reset()
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
    const id = this.$route.params.id
    if (id && id !== 'new') {
      try {
        const { data } = await getCustomization(id)
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
