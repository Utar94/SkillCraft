<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="nature"
      :createdAt="new Date(nature.createdAt)"
      :deletedAt="nature.deletedAt ? new Date(nature.deletedAt) : null"
      :updatedAt="nature.updatedAt ? new Date(nature.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <name-field v-model="name" />
        <b-row>
          <attribute-select class="col" required v-model="attribute" />
          <feat-select class="col" required v-model="featId" />
        </b-row>
        <description-field v-model="description" />
        <div class="my-2">
          <icon-submit v-if="nature" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'NatureList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import FeatSelect from './FeatSelect.vue'
import { createNature, getNature, updateNature } from '@/api/natures'

export default {
  components: {
    FeatSelect
  },
  data: () => ({
    attribute: null,
    description: null,
    featId: null,
    loading: false,
    name: null,
    nature: null
  }),
  computed: {
    hasChanges() {
      return (
        this.name !== this.nature?.name ||
        this.attribute !== this.nature?.attribute ||
        this.featId !== this.nature?.featId ||
        (this.description ?? '') !== (this.nature?.description ?? '')
      )
    },
    title() {
      return this.nature?.name ?? this.$i18n.t('nature.title')
    }
  },
  methods: {
    setModel(model) {
      this.nature = model
      this.attribute = model.attribute
      this.description = model.description
      this.featId = model.feat.id
      this.name = model.name
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.aliasConflict = false
        try {
          if (await this.$refs.form.validate()) {
            if (this.nature) {
              const { data } = await updateNature(this.nature.id, {
                attribute: this.attribute,
                description: this.description,
                featId: this.featId,
                name: this.name
              })
              this.setModel(data)
              this.toast('success', 'nature.updated')
            } else {
              const { data } = await createNature({
                attribute: this.attribute,
                description: this.description,
                featId: this.featId,
                name: this.name
              })
              this.setModel(data)
              this.toast('success', 'nature.created')
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
        const { data } = await getNature(id)
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
