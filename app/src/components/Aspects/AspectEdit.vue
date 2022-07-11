<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="aspect"
      :createdAt="new Date(aspect.createdAt)"
      :deletedAt="aspect.deletedAt ? new Date(aspect.deletedAt) : null"
      :updatedAt="aspect.updatedAt ? new Date(aspect.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <name-field v-model="name" />
        <b-row>
          <attribute-select
            class="col"
            :exclude="clean([mandatoryAttribute2, optionalAttribute1, optionalAttribute2])"
            id="mandatoryAttribute1"
            label="aspect.attribute.mandatory"
            required
            v-model="mandatoryAttribute1"
          />
          <attribute-select
            class="col"
            :exclude="clean([mandatoryAttribute1, optionalAttribute1, optionalAttribute2])"
            id="mandatoryAttribute2"
            label="aspect.attribute.mandatory"
            required
            v-model="mandatoryAttribute2"
          />
          <attribute-select
            class="col"
            :exclude="clean([mandatoryAttribute1, mandatoryAttribute2, optionalAttribute2])"
            id="optionalAttribute1"
            label="aspect.attribute.optional"
            required
            v-model="optionalAttribute1"
          />
          <attribute-select
            class="col"
            :exclude="clean([mandatoryAttribute1, mandatoryAttribute2, optionalAttribute1])"
            id="optionalAttribute2"
            label="aspect.attribute.optional"
            required
            v-model="optionalAttribute2"
          />
        </b-row>
        <b-row>
          <skill-select class="col" :exclude="clean([skill2])" id="skill1" required v-model="skill1" />
          <skill-select class="col" :exclude="clean([skill1])" id="skill2" required v-model="skill2" />
        </b-row>
        <description-field v-model="description" />
        <div class="my-2">
          <icon-submit v-if="aspect" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'AspectList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { createAspect, getAspect, updateAspect } from '@/api/aspects'

export default {
  data: () => ({
    aspect: null,
    description: null,
    loading: false,
    mandatoryAttribute1: null,
    mandatoryAttribute2: null,
    name: null,
    optionalAttribute1: null,
    optionalAttribute2: null,
    skill1: null,
    skill2: null
  }),
  computed: {
    hasChanges() {
      return (
        (this.name ?? '') !== (this.aspect?.name ?? '') ||
        this.mandatoryAttribute1 !== (this.aspect?.mandatoryAttribute1 ?? null) ||
        this.mandatoryAttribute2 !== (this.aspect?.mandatoryAttribute2 ?? null) ||
        this.optionalAttribute1 !== (this.aspect?.optionalAttribute1 ?? null) ||
        this.optionalAttribute2 !== (this.aspect?.optionalAttribute2 ?? null) ||
        this.skill1 !== (this.aspect?.skill1 ?? null) ||
        this.skill2 !== (this.aspect?.skill2 ?? null) ||
        (this.description ?? '') !== (this.aspect?.description ?? '')
      )
    },
    payload() {
      return {
        description: this.description,
        mandatoryAttribute1: this.mandatoryAttribute1,
        mandatoryAttribute2: this.mandatoryAttribute2,
        name: this.name,
        optionalAttribute1: this.optionalAttribute1,
        optionalAttribute2: this.optionalAttribute2,
        skill1: this.skill1,
        skill2: this.skill2
      }
    },
    title() {
      return this.aspect?.name ?? this.$i18n.t('aspect.title')
    }
  },
  methods: {
    clean(values) {
      return values.filter(value => typeof value !== 'undefined' && value !== null)
    },
    setModel(model) {
      this.aspect = model
      this.description = model.description
      this.mandatoryAttribute1 = model.mandatoryAttribute1
      this.mandatoryAttribute2 = model.mandatoryAttribute2
      this.name = model.name
      this.optionalAttribute1 = model.optionalAttribute1
      this.optionalAttribute2 = model.optionalAttribute2
      this.skill1 = model.skill1
      this.skill2 = model.skill2
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.aspect) {
              const { data } = await updateAspect(this.aspect.id, this.payload)
              this.setModel(data)
              this.toast('success', 'aspect.updated')
            } else {
              const { data } = await createAspect(this.payload)
              this.setModel(data)
              this.toast('success', 'aspect.created')
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
        const { data } = await getAspect(id)
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
