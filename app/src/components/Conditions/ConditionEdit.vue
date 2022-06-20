<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="condition"
      :createdAt="new Date(condition.createdAt)"
      :deletedAt="condition.deletedAt ? new Date(condition.deletedAt) : null"
      :updatedAt="condition.updatedAt ? new Date(condition.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <b-row>
          <name-field class="col" v-model="name" />
          <form-field class="col" id="maxLevel" label="condition.maxLevel.label" :maxValue="6" :minValue="0" type="number" v-model.number="maxLevel" />
        </b-row>
        <description-field v-model="description" />
        <div class="my-2">
          <icon-submit v-if="condition" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'ConditionList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { createCondition, getCondition, updateCondition } from '@/api/conditions'

export default {
  data: () => ({
    condition: null,
    description: null,
    loading: false,
    maxLevel: 0,
    name: null
  }),
  computed: {
    hasChanges() {
      return (
        (this.name ?? '') !== (this.condition?.name ?? '') ||
        this.maxLevel !== (this.condition?.maxLevel ?? 0) ||
        (this.description ?? '') !== (this.condition?.description ?? '')
      )
    },
    payload() {
      return {
        description: this.description,
        maxLevel: this.maxLevel,
        name: this.name
      }
    },
    title() {
      return this.condition?.name ?? this.$i18n.t('condition.title')
    }
  },
  methods: {
    setModel(model) {
      this.condition = model
      this.description = model.description
      this.maxLevel = model.maxLevel
      this.name = model.name
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.aliasConflict = false
        try {
          if (await this.$refs.form.validate()) {
            if (this.condition) {
              const { data } = await updateCondition(this.condition.id, this.payload)
              this.setModel(data)
              this.toast('success', 'condition.updated')
            } else {
              const { data } = await createCondition(this.payload)
              this.setModel(data)
              this.toast('success', 'condition.created')
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
        const { data } = await getCondition(id)
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
