<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="talent"
      :createdAt="new Date(talent.createdAt)"
      :deletedAt="talent.deletedAt ? new Date(talent.deletedAt) : null"
      :updatedAt="talent.updatedAt ? new Date(talent.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <name-field v-model="name" />
        <b-row>
          <tier-select class="col" :disabled="Boolean(talent)" :required="!talent" v-model="tier" />
          <talent-select
            v-if="tier !== null"
            class="col"
            id="requiredTalentId"
            :exclude="talent ? [talent.id] : []"
            :maxTier="tier"
            v-model="requiredTalentId"
          />
          <skill-select class="col" label="talent.skillTraining" v-model="skill" />
        </b-row>
        <b-form-group>
          <b-form-checkbox v-model="multipleAcquisition">{{ $t('talents.multipleAcquisition.label') }}</b-form-checkbox>
        </b-form-group>
        <description-field v-model="description" />
        <div class="my-2">
          <icon-submit v-if="talent" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'TalentList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import TalentSelect from './TalentSelect.vue'
import TierSelect from './TierSelect.vue'
import { createTalent, getTalent, updateTalent } from '@/api/talents'

export default {
  components: {
    TalentSelect,
    TierSelect
  },
  data: () => ({
    description: null,
    loading: false,
    multipleAcquisition: false,
    requiredTalentId: null,
    skill: null,
    talent: null,
    tier: null,
    name: null
  }),
  computed: {
    hasChanges() {
      return (
        (this.name ?? '') !== (this.talent?.name ?? '') ||
        this.tier !== (this.talent?.tier ?? null) ||
        this.requiredTalentId !== (this.talent?.requiredTalent?.id ?? null) ||
        this.skill !== (this.talent?.skill ?? null) ||
        this.multipleAcquisition !== (this.talent?.multipleAcquisition ?? false) ||
        (this.description ?? '') !== (this.talent?.description ?? '')
      )
    },
    payload() {
      const payload = {
        description: this.description,
        multipleAcquisition: this.multipleAcquisition,
        name: this.name,
        requiredTalentId: this.requiredTalentId,
        skill: this.skill
      }
      if (!this.talent) {
        payload.tier = this.tier
      }
      return payload
    },
    title() {
      return this.talent?.name ?? this.$i18n.t('talent.title')
    }
  },
  methods: {
    setModel(model) {
      this.talent = model
      this.description = model.description
      this.multipleAcquisition = model.multipleAcquisition
      this.name = model.name
      this.requiredTalentId = model.requiredTalent?.id ?? null
      this.skill = model.skill
      this.tier = model.tier
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.aliasConflict = false
        try {
          if (await this.$refs.form.validate()) {
            if (this.talent) {
              const { data } = await updateTalent(this.talent.id, this.payload)
              this.setModel(data)
              this.toast('success', 'talent.updated')
            } else {
              const { data } = await createTalent(this.payload)
              this.setModel(data)
              this.toast('success', 'talent.created')
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
        const { data } = await getTalent(id)
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
