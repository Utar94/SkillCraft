<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="caste"
      :createdAt="new Date(caste.createdAt)"
      :deletedAt="caste.deletedAt ? new Date(caste.deletedAt) : null"
      :updatedAt="caste.updatedAt ? new Date(caste.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <name-field v-model="name" />
        <b-row>
          <skill-select class="col" required v-model="skill" />
          <form-field
            class="col"
            id="wealthRoll"
            label="caste.wealthRoll.label"
            placeholder="caste.wealthRoll.placeholder"
            :rules="{ regex: /^\d{1,2}d\d{1,2}(\+\d{1,3})?$/ }"
            v-model="wealthRoll"
          />
        </b-row>
        <description-field v-model="description" />
        <div class="my-2">
          <icon-submit v-if="caste" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'CasteList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { createCaste, getCaste, updateCaste } from '@/api/castes'

export default {
  data: () => ({
    caste: null,
    description: null,
    loading: false,
    name: null,
    skill: null,
    wealthRoll: null
  }),
  computed: {
    hasChanges() {
      return (
        (this.name ?? '') !== (this.caste?.name ?? '') ||
        this.skill !== (this.caste?.skill ?? null) ||
        (this.wealthRoll ?? '') !== (this.caste?.wealthRoll ?? '') ||
        (this.description ?? '') !== (this.caste?.description ?? '')
      )
    },
    payload() {
      return {
        description: this.description,
        name: this.name,
        skill: this.skill,
        wealthRoll: this.wealthRoll
      }
    },
    title() {
      return this.caste?.name ?? this.$i18n.t('caste.title')
    }
  },
  methods: {
    setModel(model) {
      this.caste = model
      this.description = model.description
      this.name = model.name
      this.skill = model.skill
      this.wealthRoll = model.wealthRoll
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.aliasConflict = false
        try {
          if (await this.$refs.form.validate()) {
            if (this.caste) {
              const { data } = await updateCaste(this.caste.id, this.payload)
              this.setModel(data)
              this.toast('success', 'caste.updated')
            } else {
              const { data } = await createCaste(this.payload)
              this.setModel(data)
              this.toast('success', 'caste.created')
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
        const { data } = await getCaste(id)
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
