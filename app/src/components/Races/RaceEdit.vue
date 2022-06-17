<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="race"
      :createdAt="new Date(race.createdAt)"
      :deletedAt="race.deletedAt ? new Date(race.deletedAt) : null"
      :updatedAt="race.updatedAt ? new Date(race.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('race.general')" active>
            <name-field v-model="name" />
            <description-field v-model="description" />
          </b-tab>
          <b-tab :title="$t('race.attributes.tab')">
            <b-row>
              <attribute-bonus-field class="col" attribute="Agility" v-model.number="attributes.Agility" />
              <attribute-bonus-field class="col" attribute="Coordination" v-model.number="attributes.Coordination" />
              <attribute-bonus-field class="col" attribute="Intellect" v-model.number="attributes.Intellect" />
              <attribute-bonus-field class="col" attribute="Mind" v-model.number="attributes.Mind" />
              <attribute-bonus-field class="col" attribute="Presence" v-model.number="attributes.Presence" />
              <attribute-bonus-field class="col" attribute="Sensitivity" v-model.number="attributes.Sensitivity" />
              <attribute-bonus-field class="col" attribute="Vigor" v-model.number="attributes.Vigor" />
            </b-row>
            <form-field id="extraAttributes" label="race.attributes.extra" :maxValue="3" :minValue="0" type="number" v-model.number="extraAttributes" />
            <form-textarea
              id="attributesText"
              label="description.label"
              :maxLength="1000"
              placeholder="race.attributes.text"
              :rows="5"
              v-model="attributesText"
            />
          </b-tab>
          <b-tab :title="$t('race.languages.tab')">
            <language-select :disabled="!languageOptions.length" label="race.languages.add" :options="languageOptions" :value="null" @input="addLanguage" />
            <tag-list label="race.languages.tab" :tags="languageTags" @remove="removeLanguage" />
            <form-field id="extraLanguages" label="race.languages.extra" :maxValue="3" :minValue="0" type="number" v-model.number="extraLanguages" />
            <form-textarea id="languagesText" label="description.label" :maxLength="1000" placeholder="race.languages.text" :rows="5" v-model="languagesText" />
          </b-tab>
          <b-tab :title="$t('race.speeds.tab')">
            <b-row>
              <racial-speed-field class="col" speedType="Walk" v-model.number="speeds.Walk" />
              <racial-speed-field class="col" speedType="Burrow" v-model.number="speeds.Burrow" />
              <racial-speed-field class="col" speedType="Climb" v-model.number="speeds.Climb" />
              <racial-speed-field class="col" speedType="Fly" v-model.number="speeds.Fly" />
              <racial-speed-field class="col" speedType="Swim" v-model.number="speeds.Swim" />
            </b-row>
            <form-textarea id="speedText" label="description.label" :maxLength="1000" placeholder="race.speeds.text" :rows="5" v-model="speedText" />
          </b-tab>
        </b-tabs>
        <div class="my-2">
          <icon-submit v-if="race" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'RaceList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
/* TODO(fpion):
 * Names; NamesText
 * AgeThresholds, Size, StatureRoll, WeightRolls; AgeText, SizeText, WeightText
 * Traits; TraitsText
 * SubraceText
 */

import AttributeBonusField from './AttributeBonusField.vue'
import LanguageSelect from '../Languages/LanguageSelect.vue'
import RacialSpeedField from './RacialSpeedField.vue'
import Vue from 'vue'
import { createRace, getRace, updateRace } from '@/api/races'
import { getLanguages } from '@/api/languages'

export default {
  components: {
    AttributeBonusField,
    LanguageSelect,
    RacialSpeedField
  },
  data: () => ({
    attributes: {
      Agility: 0,
      Coordination: 0,
      Intellect: 0,
      Mind: 0,
      Presence: 0,
      Sensitivity: 0,
      Vigor: 0
    },
    attributesText: null,
    description: null,
    extraAttributes: 0,
    extraLanguages: 0,
    languages: [],
    languagesText: null,
    loading: false,
    name: null,
    race: null,
    speedText: null,
    speeds: {
      Burrow: 0,
      Climb: 0,
      Fly: 0,
      Swim: 0,
      Walk: 0
    },
    worldLanguages: []
  }),
  computed: {
    hasChanges() {
      return (
        // General
        this.name !== this.race?.name ||
        (this.description ?? '') !== (this.race?.description ?? '') ||
        // Attributes
        this.attributes.Agility !== (this.race?.attributes.find(({ attribute }) => attribute === 'Agility')?.bonus ?? 0) ||
        this.attributes.Coordination !== (this.race?.attributes.find(({ attribute }) => attribute === 'Coordination')?.bonus ?? 0) ||
        this.attributes.Intellect !== (this.race?.attributes.find(({ attribute }) => attribute === 'Intellect')?.bonus ?? 0) ||
        this.attributes.Mind !== (this.race?.attributes.find(({ attribute }) => attribute === 'Mind')?.bonus ?? 0) ||
        this.attributes.Presence !== (this.race?.attributes.find(({ attribute }) => attribute === 'Presence')?.bonus ?? 0) ||
        this.attributes.Sensitivity !== (this.race?.attributes.find(({ attribute }) => attribute === 'Sensitivity')?.bonus ?? 0) ||
        this.attributes.Vigor !== (this.race?.attributes.find(({ attribute }) => attribute === 'Vigor')?.bonus ?? 0) ||
        this.extraAttributes !== this.race?.extraAttributes ||
        (this.attributesText ?? '') !== (this.race?.attributesText ?? '') ||
        // Languages
        JSON.stringify(this.orderBy(this.languageIds)) !== JSON.stringify(this.orderBy(this.race?.languages ?? [], 'id').map(({ id }) => id)) ||
        this.extraLanguages !== this.race?.extraLanguages ||
        (this.languagesText ?? '') !== (this.race?.languagesText ?? '') ||
        // Speeds
        this.speeds.Burrow !== (this.race?.speeds.find(({ type }) => type === 'Burrow')?.value ?? 0) ||
        this.speeds.Climb !== (this.race?.speeds.find(({ type }) => type === 'Climb')?.value ?? 0) ||
        this.speeds.Fly !== (this.race?.speeds.find(({ type }) => type === 'Fly')?.value ?? 0) ||
        this.speeds.Swim !== (this.race?.speeds.find(({ type }) => type === 'Swim')?.value ?? 0) ||
        this.speeds.Walk !== (this.race?.speeds.find(({ type }) => type === 'Walk')?.value ?? 0) ||
        (this.speedText ?? '') !== (this.race?.speedText ?? '')
      )
    },
    languageIds() {
      return this.languages.filter(({ status }) => status !== 'removed').map(({ id }) => id)
    },
    languageOptions() {
      return this.worldLanguages
        .filter(({ id }) => !this.languageIds.includes(id))
        .map(({ id, name }) => ({
          text: name,
          value: id
        }))
    },
    languageTags() {
      return this.orderBy(this.languages, 'name').map(({ id, name, status }) => ({
        disabled: status === 'removed',
        text: name,
        value: id,
        variant: status === 'added' ? 'success' : status === 'removed' ? 'danger' : ''
      }))
    },
    payload() {
      const payload = {
        attributes: Object.entries(this.attributes)
          .filter(([, bonus]) => bonus > 0)
          .map(([attribute, bonus]) => ({ attribute, bonus })),
        attributesText: this.attributesText,
        description: this.description,
        extraAttributes: this.extraAttributes,
        extraLanguages: this.extraLanguages,
        languageIds: this.languageIds,
        languagesText: this.languagesText,
        name: this.name,
        speedText: this.speedText,
        speeds: Object.entries(this.speeds)
          .filter(([, value]) => value > 0)
          .map(([type, value]) => ({ type, value }))
      }
      return payload
    },
    title() {
      return this.race?.name ?? this.$i18n.t('race.title')
    }
  },
  methods: {
    addLanguage(value) {
      const index = this.languages.findIndex(({ id }) => id === value)
      if (index >= 0) {
        const language = this.languages[index]
        if (language.status === 'removed') {
          delete language.status
          Vue.set(this.languages, index, language)
          return
        }
      }
      const language = this.worldLanguages.find(({ id }) => id === value)
      if (language) {
        this.languages.push({
          ...language,
          status: 'added'
        })
      }
    },
    clean(values) {
      return values.filter(value => typeof value !== 'undefined' && value !== null)
    },
    removeLanguage({ value }) {
      const index = this.languages.findIndex(({ id }) => id === value)
      if (index >= 0) {
        const language = this.languages[index]
        if (language.status === 'added') {
          Vue.delete(this.languages, index)
        } else if (language.status !== 'removed') {
          language.status = 'removed'
          Vue.set(this.languages, index, language)
        }
      }
    },
    setModel(model) {
      this.race = model
      // General
      this.name = model.name
      this.description = model.description
      // Attributes
      this.attributes.Agility = model.attributes.find(({ attribute }) => attribute === 'Agility')?.bonus ?? 0
      this.attributes.Coordination = model.attributes.find(({ attribute }) => attribute === 'Coordination')?.bonus ?? 0
      this.attributes.Intellect = model.attributes.find(({ attribute }) => attribute === 'Intellect')?.bonus ?? 0
      this.attributes.Mind = model.attributes.find(({ attribute }) => attribute === 'Mind')?.bonus ?? 0
      this.attributes.Presence = model.attributes.find(({ attribute }) => attribute === 'Presence')?.bonus ?? 0
      this.attributes.Sensitivity = model.attributes.find(({ attribute }) => attribute === 'Sensitivity')?.bonus ?? 0
      this.attributes.Vigor = model.attributes.find(({ attribute }) => attribute === 'Vigor')?.bonus ?? 0
      this.extraAttributes = model.extraAttributes
      this.attributesText = model.attributesText
      // Languages
      this.languages = [...model.languages]
      this.extraLanguages = model.extraLanguages
      this.languagesText = model.languagesText
      // Speeds
      this.speeds.Burrow = model.speeds.find(({ type }) => type === 'Burrow')?.value ?? 0
      this.speeds.Climb = model.speeds.find(({ type }) => type === 'Climb')?.value ?? 0
      this.speeds.Fly = model.speeds.find(({ type }) => type === 'Fly')?.value ?? 0
      this.speeds.Swim = model.speeds.find(({ type }) => type === 'Swim')?.value ?? 0
      this.speeds.Walk = model.speeds.find(({ type }) => type === 'Walk')?.value ?? 0
      this.speedText = model.speedText
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.aliasConflict = false
        try {
          if (await this.$refs.form.validate()) {
            if (this.race) {
              const { data } = await updateRace(this.race.id, this.payload)
              this.setModel(data)
              this.toast('success', 'race.updated')
            } else {
              const { data } = await createRace(this.payload)
              this.setModel(data)
              this.toast('success', 'race.created')
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
        const { data } = await getRace(id)
        this.setModel(data)
      } catch (e) {
        if (e.status === 404) {
          return this.$router.push({ name: 'NotFound' })
        }
        this.handleError(e)
      }
    }
    try {
      const { data } = await getLanguages({ deleted: false, sort: 'Name', desc: false })
      this.worldLanguages = data.items
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
