<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="cclass"
      :createdAt="new Date(cclass.createdAt)"
      :deletedAt="cclass.deletedAt ? new Date(cclass.deletedAt) : null"
      :updatedAt="cclass.updatedAt ? new Date(cclass.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('class.general')" active>
            <b-row>
              <name-field class="col" v-model="name" />
              <tier-select class="col" :disabled="Boolean(cclass)" :options="[1, 2, 3]" :required="!cclass" v-model="tier" />
            </b-row>
            <talent-select
              :disabled="tier === null"
              id="uniqueTalent"
              label="class.uniqueTalent.label"
              placeholder="class.uniqueTalent.placeholder"
              required
              :tiers="[tier - 1]"
              v-model="uniqueTalentId"
            />
            <description-field v-model="description" />
          </b-tab>
          <b-tab :title="$t('class.acquisition.tab')">
            <icon-button class="mb-2" icon="plus" text="actions.add" variant="success" v-b-modal.selectTalent />
            <select-talent-modal :exclude="talentIds" :tier="tier" @ok="addTalent" />
            <tag-list label="class.acquisition.talents" :tags="talentTags" @remove="removeTalent" />
            <form-field
              id="otherTalentsText"
              label="class.otherTalentsText.label"
              :maxLength="100"
              placeholder="class.otherTalentsText.placeholder"
              v-model="otherTalentsText"
            />
            <form-textarea
              id="otherRequirements"
              label="class.otherRequirements.label"
              :maxLength="1000"
              placeholder="class.otherRequirements.placeholder"
              :rows="5"
              v-model="otherRequirements"
            />
          </b-tab>
        </b-tabs>
        <div class="my-2">
          <icon-submit v-if="cclass" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'ClassList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import SelectTalentModal from './SelectTalentModal.vue'
import TalentSelect from '../Talents/TalentSelect.vue'
import Vue from 'vue'
import { createClass, getClass, updateClass } from '@/api/classes'

export default {
  components: {
    SelectTalentModal,
    TalentSelect
  },
  data: () => ({
    cclass: null,
    description: null,
    loading: false,
    name: null,
    otherRequirements: null,
    otherTalentsText: null,
    talents: [],
    tier: null,
    uniqueTalentId: null
  }),
  computed: {
    hasChanges() {
      return (
        (this.name ?? '') !== (this.cclass?.name ?? '') ||
        this.tier !== (this.cclass?.tier ?? null) ||
        this.uniqueTalentId !== (this.cclass?.uniqueTalent?.id ?? null) ||
        (this.description ?? '') !== (this.cclass?.description ?? '') ||
        JSON.stringify(this.serialize(this.talents)) !== JSON.stringify(this.serialize(this.cclass?.talents ?? [])) ||
        (this.otherRequirements ?? '') !== (this.cclass?.otherRequirements ?? '') ||
        (this.otherTalentsText ?? '') !== (this.cclass?.otherTalentsText ?? '')
      )
    },
    payload() {
      const payload = {
        description: this.description,
        name: this.name,
        otherRequirements: this.otherRequirements,
        otherTalentsText: this.otherTalentsText,
        talents: this.talents.map(({ mandatory, talent }) => ({ mandatory, talentId: talent.id })),
        uniqueTalentId: this.uniqueTalentId
      }
      if (!this.cclass) {
        payload.tier = this.tier
      }
      return payload
    },
    talentIds() {
      return this.talents.map(({ talent }) => talent.id)
    },
    talentTags() {
      return this.orderBy(
        this.talents.map(({ mandatory, talent }) => ({
          text: talent.name,
          value: talent.id,
          variant: mandatory ? 'warning' : 'primary'
        })),
        'text'
      )
    },
    title() {
      return this.cclass?.name ?? this.$i18n.t('class.title')
    }
  },
  methods: {
    addTalent({ callback, mandatory, talent }) {
      this.talents.push({ mandatory, talent })
      if (callback) {
        callback()
      }
    },
    removeTalent({ value }) {
      const index = this.talents.findIndex(({ talent }) => talent.id === value)
      if (index >= 0) {
        Vue.delete(this.talents, index)
      }
    },
    serialize(values) {
      return this.orderBy(values.map(({ mandatory, talent }) => `${talent.id}|${mandatory}`))
    },
    setModel(model) {
      this.cclass = model
      this.description = model.description
      this.name = model.name
      this.otherRequirements = model.otherRequirements
      this.otherTalentsText = model.otherTalentsText
      this.talents = model.talents.map(talent => ({ ...talent }))
      this.tier = model.tier
      this.uniqueTalentId = model.uniqueTalent.id
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.aliasConflict = false
        try {
          if (await this.$refs.form.validate()) {
            if (this.cclass) {
              const { data } = await updateClass(this.cclass.id, this.payload)
              this.setModel(data)
              this.toast('success', 'class.updated')
            } else {
              const { data } = await createClass(this.payload)
              this.setModel(data)
              this.toast('success', 'class.created')
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
        const { data } = await getClass(id)
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
