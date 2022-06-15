<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="education"
      :createdAt="new Date(education.createdAt)"
      :deletedAt="education.deletedAt ? new Date(education.deletedAt) : null"
      :updatedAt="education.updatedAt ? new Date(education.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <name-field v-model="name" />
        <b-row>
          <skill-select class="col" required v-model="skill" />
          <form-field
            class="col"
            id="wealthMultiplier"
            label="education.wealthMultiplier"
            :maxValue="12"
            :minValue="0"
            type="number"
            v-model.number="wealthMultiplier"
          />
        </b-row>
        <description-field v-model="description" />
        <div class="my-2">
          <icon-submit v-if="education" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'EducationList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { createEducation, getEducation, updateEducation } from '@/api/educations'

export default {
  data: () => ({
    education: null,
    description: null,
    loading: false,
    name: null,
    skill: null,
    wealthMultiplier: 0
  }),
  computed: {
    hasChanges() {
      return (
        this.name !== this.education?.name ||
        this.skill !== this.education?.skill ||
        this.wealthMultiplier !== (this.education?.wealthMultiplier ?? 0) ||
        (this.description ?? '') !== (this.education?.description ?? '')
      )
    },
    title() {
      return this.education?.name ?? this.$i18n.t('education.title')
    }
  },
  methods: {
    clean(values) {
      return values.filter(value => typeof value !== 'undefined' && value !== null)
    },
    setModel(model) {
      this.education = model
      this.description = model.description
      this.name = model.name
      this.skill = model.skill
      this.wealthMultiplier = model.wealthMultiplier ?? 0
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.aliasConflict = false
        try {
          if (await this.$refs.form.validate()) {
            if (this.education) {
              const { data } = await updateEducation(this.education.id, {
                description: this.description,
                name: this.name,
                skill: this.skill,
                wealthMultiplier: this.wealthMultiplier || null
              })
              this.setModel(data)
              this.toast('success', 'education.updated')
            } else {
              const { data } = await createEducation({
                description: this.description,
                name: this.name,
                skill: this.skill,
                wealthMultiplier: this.wealthMultiplier || null
              })
              this.setModel(data)
              this.toast('success', 'education.created')
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
        const { data } = await getEducation(id)
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
