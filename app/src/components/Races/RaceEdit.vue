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
        <div class="my-2">
          <icon-submit v-if="race" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="back" />
        </div>
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
            <race-text id="attributesText" :placeholder="`${type}.text.attributes`" v-model="attributesText" />
          </b-tab>
          <b-tab :title="$t('race.languages.tab')">
            <language-select :disabled="!languageOptions.length" label="race.languages.add" :options="languageOptions" :value="null" @input="addLanguage" />
            <tag-list label="race.languages.tab" :tags="languageTags" @remove="removeLanguage" />
            <form-field id="extraLanguages" label="race.languages.extra" :maxValue="3" :minValue="0" type="number" v-model.number="extraLanguages" />
            <race-text id="languagesText" :placeholder="`${type}.text.languages`" v-model="languagesText" />
          </b-tab>
          <b-tab :title="$t('race.physical.tab')">
            <h4 v-t="'race.physical.size.title'" />
            <b-row>
              <size-category-select class="col" label="race.physical.size.category" required v-model="size" />
              <form-field
                class="col"
                id="statureRoll"
                label="race.physical.size.statureRoll.label"
                placeholder="race.physical.size.statureRoll.placeholder"
                :rules="{ regex: /^\d{1,2}d\d{1,2}(\+\d{1,3})?$/ }"
                v-model="statureRoll"
              />
            </b-row>
            <race-text id="sizeText" :placeholder="`${type}.text.size`" v-model="sizeText" />
            <h4 v-t="'race.physical.weight.title'" />
            <b-row>
              <weight-field class="col" id="skinny" label="race.physical.weight.skinny" v-model="weightRolls.skinny" />
              <weight-field
                class="col"
                :disabled="!weightRolls.skinny"
                id="thin"
                label="race.physical.weight.thin"
                :required="Boolean(weightRolls.skinny)"
                v-model="weightRolls.thin"
              />
              <weight-field
                class="col"
                :disabled="!weightRolls.thin"
                id="normal"
                label="race.physical.weight.normal"
                :required="Boolean(weightRolls.thin)"
                v-model="weightRolls.normal"
              />
              <weight-field
                class="col"
                :disabled="!weightRolls.normal"
                id="overweight"
                label="race.physical.weight.overweight"
                :required="Boolean(weightRolls.normal)"
                v-model="weightRolls.overweight"
              />
              <weight-field
                class="col"
                :disabled="!weightRolls.overweight"
                id="obese"
                label="race.physical.weight.obese"
                :required="Boolean(weightRolls.overweight)"
                v-model="weightRolls.obese"
              />
            </b-row>
            <race-text id="weightText" :placeholder="`${type}.text.weight`" v-model="weightText" />
            <h4 v-t="'race.physical.age.title'" />
            <b-row>
              <form-field class="col" disabled id="child" label="race.physical.age.child" type="number" :value="0" />
              <form-field class="col" id="teenager" label="race.physical.age.teenager" :minValue="0" type="number" v-model.number="ageThresholds.teenager" />
              <form-field
                class="col"
                :disabled="ageThresholds.teenager === 0"
                id="adult"
                label="race.physical.age.adult"
                :minValue="ageThresholds.teenager > 0 ? ageThresholds.teenager + 1 : null"
                :required="ageThresholds.teenager > 0"
                type="number"
                v-model.number="ageThresholds.adult"
              />
              <form-field
                class="col"
                :disabled="ageThresholds.adult === 0"
                id="mature"
                label="race.physical.age.mature"
                :minValue="ageThresholds.adult > 0 ? ageThresholds.adult + 1 : null"
                :required="ageThresholds.adult > 0"
                type="number"
                v-model.number="ageThresholds.mature"
              />
              <form-field
                class="col"
                :disabled="ageThresholds.mature === 0"
                id="venerable"
                label="race.physical.age.venerable"
                :minValue="ageThresholds.mature > 0 ? ageThresholds.mature + 1 : null"
                :required="ageThresholds.mature > 0"
                type="number"
                v-model.number="ageThresholds.venerable"
              />
            </b-row>
            <race-text id="ageText" :placeholder="`${type}.text.age`" v-model="ageText" />
          </b-tab>
          <b-tab :title="$t('race.speeds.tab')">
            <b-row>
              <racial-speed-field class="col" speedType="Walk" v-model.number="speeds.Walk" />
              <racial-speed-field class="col" speedType="Burrow" v-model.number="speeds.Burrow" />
              <racial-speed-field class="col" speedType="Climb" v-model.number="speeds.Climb" />
              <racial-speed-field class="col" speedType="Fly" v-model.number="speeds.Fly" />
              <racial-speed-field class="col" speedType="Swim" v-model.number="speeds.Swim" />
            </b-row>
            <race-text id="speedText" :placeholder="`${type}.text.speeds`" v-model="speedText" />
          </b-tab>
          <b-tab :title="$t('race.traits.tab')">
            <race-text id="traitsText" :placeholder="`${type}.text.traits`" v-model="traitsText" />
            <div class="my-2">
              <icon-button class="mx-1" icon="plus" text="actions.add" variant="success" v-b-modal.newTrait />
              <trait-edit-modal id="newTrait" @ok="addTrait" />
            </div>
            <table v-if="traits.length" class="table table-striped">
              <thead>
                <tr>
                  <th scope="col" v-t="'name.label'" />
                  <th scope="col" v-t="'description.label'" />
                  <th scope="col" />
                </tr>
              </thead>
              <tbody>
                <tr v-for="(trait, index) in traits" :key="index">
                  <td>
                    {{ trait.name }}
                    <trait-status v-if="trait.status" :status="trait.status" />
                  </td>
                  <td v-text="shortify(trait.description, 100)" />
                  <td>
                    <icon-button
                      class="mx-1"
                      :disabled="trait.status !== 'removed' && trait.status !== 'updated'"
                      icon="undo"
                      variant="warning"
                      @click="restoreTrait(index)"
                    />
                    <icon-button class="mx-1" :disabled="trait.status === 'removed'" icon="edit" variant="primary" v-b-modal="`editTrait_${index}`" />
                    <icon-button class="mx-1" :disabled="trait.status === 'removed'" icon="times" variant="danger" @click="removeTrait(index)" />
                  </td>
                  <trait-edit-modal :id="`editTrait_${index}`" :trait="trait" @ok="updateTrait(index, $event)" />
                </tr>
              </tbody>
            </table>
            <p v-else v-t="'race.traits.empty'" />
          </b-tab>
          <b-tab v-if="race && type === 'race'" :title="$t('race.people.tab')">
            <race-text id="peopleText" placeholder="race.text.people" v-model="peopleText" />
            <div class="my-2">
              <icon-button icon="plus" text="actions.create" :to="{ name: 'PeopleEdit', params: { id: 'new', raceId: race.id } }" variant="success" />
            </div>
            <table v-if="people.length" class="table table-striped">
              <thead>
                <tr>
                  <th scope="col" v-t="'name.label'" />
                  <th scope="col" v-t="'updatedAt'" />
                  <th scope="col" />
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in sortedPeople" :key="item.id">
                  <td>
                    <router-link :to="{ name: 'PeopleEdit', params: { id: item.id, raceId: race.id } }" v-text="item.name" />
                  </td>
                  <td>{{ $d(new Date(item.updatedAt || item.createdAt), 'medium') }}</td>
                  <td>
                    <icon-button class="mx-1" icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`deletePeople_${item.id}`" />
                    <delete-modal
                      confirm="people.delete.confirm"
                      :disabled="loading"
                      :displayName="item.name"
                      :id="`deletePeople_${item.id}`"
                      :loading="loading"
                      title="people.delete.title"
                      @ok="_delete(item, $event)"
                    />
                  </td>
                </tr>
              </tbody>
            </table>
            <p v-else v-t="'race.people.empty'" />
          </b-tab>
        </b-tabs>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
// TODO(fpion): Names; NamesText

import AttributeBonusField from './AttributeBonusField.vue'
import LanguageSelect from '../Languages/LanguageSelect.vue'
import RaceText from './RaceText.vue'
import RacialSpeedField from './RacialSpeedField.vue'
import TraitEditModal from './TraitEditModal.vue'
import TraitStatus from './TraitStatus.vue'
import Vue from 'vue'
import WeightField from './WeightField.vue'
import { createRace, deleteRace, getPeople, getRace, updateRace } from '@/api/races'
import { getLanguages } from '@/api/languages'

export default {
  components: {
    AttributeBonusField,
    LanguageSelect,
    RaceText,
    RacialSpeedField,
    TraitEditModal,
    TraitStatus,
    WeightField
  },
  data: () => ({
    ageThresholds: {
      teenager: 0,
      adult: 0,
      mature: 0,
      venerable: 0
    },
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
    ageText: null,
    description: null,
    extraAttributes: 0,
    extraLanguages: 0,
    languages: [],
    languagesText: null,
    loading: false,
    name: null,
    people: [],
    peopleText: null,
    race: null,
    size: 'Medium',
    sizeText: null,
    speedText: null,
    speeds: {
      Burrow: 0,
      Climb: 0,
      Fly: 0,
      Swim: 0,
      Walk: 0
    },
    statureRoll: null,
    traits: [],
    traitsText: null,
    weightRolls: {
      skinny: null,
      normal: null,
      obese: null,
      overweight: null,
      thin: null
    },
    weightText: null,
    worldLanguages: []
  }),
  computed: {
    back() {
      return this.type === 'people' ? { name: 'RaceEdit', params: { id: this.$route.params.raceId } } : { name: 'RaceList' }
    },
    hasChanges() {
      return (
        // General
        (this.name ?? '') !== (this.race?.name ?? '') ||
        (this.description ?? '') !== (this.race?.description ?? '') ||
        // Attributes
        this.attributes.Agility !== (this.race?.attributes.find(({ attribute }) => attribute === 'Agility')?.bonus ?? 0) ||
        this.attributes.Coordination !== (this.race?.attributes.find(({ attribute }) => attribute === 'Coordination')?.bonus ?? 0) ||
        this.attributes.Intellect !== (this.race?.attributes.find(({ attribute }) => attribute === 'Intellect')?.bonus ?? 0) ||
        this.attributes.Mind !== (this.race?.attributes.find(({ attribute }) => attribute === 'Mind')?.bonus ?? 0) ||
        this.attributes.Presence !== (this.race?.attributes.find(({ attribute }) => attribute === 'Presence')?.bonus ?? 0) ||
        this.attributes.Sensitivity !== (this.race?.attributes.find(({ attribute }) => attribute === 'Sensitivity')?.bonus ?? 0) ||
        this.attributes.Vigor !== (this.race?.attributes.find(({ attribute }) => attribute === 'Vigor')?.bonus ?? 0) ||
        this.extraAttributes !== (this.race?.extraAttributes ?? 0) ||
        (this.attributesText ?? '') !== (this.race?.attributesText ?? '') ||
        // Languages
        JSON.stringify(this.orderBy(this.languageIds)) !== JSON.stringify(this.orderBy(this.race?.languages ?? [], 'id').map(({ id }) => id)) ||
        this.extraLanguages !== (this.race?.extraLanguages ?? 0) ||
        (this.languagesText ?? '') !== (this.race?.languagesText ?? '') ||
        // Physical
        this.size !== (this.race?.size ?? 'Medium') ||
        (this.statureRoll ?? '') !== (this.race?.statureRoll ?? '') ||
        (this.sizeText ?? '') !== (this.race?.sizeText ?? '') ||
        (this.weightRolls.skinny ?? '') !== (this.race?.weightRolls?.skinny ?? '') ||
        (this.weightRolls.thin ?? '') !== (this.race?.weightRolls?.thin ?? '') ||
        (this.weightRolls.normal ?? '') !== (this.race?.weightRolls?.normal ?? '') ||
        (this.weightRolls.overweight ?? '') !== (this.race?.weightRolls?.overweight ?? '') ||
        (this.weightRolls.obese ?? '') !== (this.race?.weightRolls?.obese ?? '') ||
        (this.weightText ?? '') !== (this.race?.weightText ?? '') ||
        this.ageThresholds.teenager !== (this.race?.ageThresholds?.teenager ?? 0) ||
        this.ageThresholds.adult !== (this.race?.ageThresholds?.adult ?? 0) ||
        this.ageThresholds.mature !== (this.race?.ageThresholds?.mature ?? 0) ||
        this.ageThresholds.venerable !== (this.race?.ageThresholds?.venerable ?? 0) ||
        (this.ageText ?? '') !== (this.race?.ageText ?? '') ||
        // Speeds
        this.speeds.Burrow !== (this.race?.speeds.find(({ type }) => type === 'Burrow')?.value ?? 0) ||
        this.speeds.Climb !== (this.race?.speeds.find(({ type }) => type === 'Climb')?.value ?? 0) ||
        this.speeds.Fly !== (this.race?.speeds.find(({ type }) => type === 'Fly')?.value ?? 0) ||
        this.speeds.Swim !== (this.race?.speeds.find(({ type }) => type === 'Swim')?.value ?? 0) ||
        this.speeds.Walk !== (this.race?.speeds.find(({ type }) => type === 'Walk')?.value ?? 0) ||
        (this.speedText ?? '') !== (this.race?.speedText ?? '') ||
        // Traits
        (this.traitsText ?? '') !== (this.race?.traitsText ?? '') ||
        this.traits.some(({ status }) => Boolean(status)) ||
        // People
        (this.peopleText ?? '') !== (this.race?.peopleText ?? '')
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
        ageThresholds: Object.values(this.ageThresholds).some(value => value > 0) ? this.ageThresholds : null,
        attributes: Object.entries(this.attributes)
          .filter(([, bonus]) => bonus > 0)
          .map(([attribute, bonus]) => ({ attribute, bonus })),
        attributesText: this.attributesText,
        ageText: this.ageText,
        description: this.description,
        extraAttributes: this.extraAttributes,
        extraLanguages: this.extraLanguages,
        languageIds: this.languageIds,
        languagesText: this.languagesText,
        name: this.name,
        size: this.size,
        sizeText: this.sizeText,
        speedText: this.speedText,
        speeds: Object.entries(this.speeds)
          .filter(([, value]) => value > 0)
          .map(([type, value]) => ({ type, value })),
        statureRoll: this.statureRoll ?? null,
        peopleText: this.peopleText,
        traits: this.traits.filter(({ status }) => status !== 'removed').map(({ id, description, name }) => ({ id, description, name })),
        traitsText: this.traitsText,
        weightRolls: Object.values(this.weightRolls).some(value => Boolean(value)) ? this.weightRolls : null,
        weightText: this.weightText
      }
      if (this.type === 'people' && !this.race) {
        payload.parentId = this.$route.params.raceId
      }
      return payload
    },
    sortedPeople() {
      return this.orderBy([...this.people], 'name')
    },
    title() {
      return this.race?.name ?? this.$i18n.t(`${this.type}.title`)
    },
    type() {
      return this.$route.name === 'PeopleEdit' ? 'people' : 'race'
    }
  },
  methods: {
    async _delete({ id }, callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          await deleteRace(id)
          const index = this.people.findIndex(people => people.id === id)
          if (index >= 0) {
            Vue.delete(this.people, index)
            this.toast('success', 'people.delete.success')
            if (typeof callback === 'function') {
              callback()
            }
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    },
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
    addTrait({ callback, description, name }) {
      this.traits.push({ description, name, status: 'added' })
      if (callback) {
        callback()
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
    removeTrait(index) {
      const trait = this.traits[index]
      if (trait.status === 'added') {
        Vue.delete(this.traits, index)
        return
      }
      trait.status = 'removed'
      Vue.set(this.traits, index, trait)
    },
    restoreTrait(index) {
      const trait = this.traits[index]
      if (trait.old) {
        for (const [key, value] of Object.entries(trait.old)) {
          trait[key] = value
        }
        delete trait.old
      }
      delete trait.status
      Vue.set(this.traits, index, trait)
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
      // Physical
      this.size = model.size
      this.statureRoll = model.statureRoll
      this.sizeText = model.sizeText
      this.weightRolls.skinny = model.weightRolls?.skinny ?? null
      this.weightRolls.thin = model.weightRolls?.thin ?? null
      this.weightRolls.normal = model.weightRolls?.normal ?? null
      this.weightRolls.overweight = model.weightRolls?.overweight ?? null
      this.weightRolls.obese = model.weightRolls?.obese ?? null
      this.weightText = model.weightText
      this.ageThresholds.teenager = model.ageThresholds?.teenager ?? 0
      this.ageThresholds.adult = model.ageThresholds?.adult ?? 0
      this.ageThresholds.mature = model.ageThresholds?.mature ?? 0
      this.ageThresholds.venerable = model.ageThresholds?.venerable ?? 0
      this.ageText = model.ageText
      // Speeds
      this.speeds.Burrow = model.speeds.find(({ type }) => type === 'Burrow')?.value ?? 0
      this.speeds.Climb = model.speeds.find(({ type }) => type === 'Climb')?.value ?? 0
      this.speeds.Fly = model.speeds.find(({ type }) => type === 'Fly')?.value ?? 0
      this.speeds.Swim = model.speeds.find(({ type }) => type === 'Swim')?.value ?? 0
      this.speeds.Walk = model.speeds.find(({ type }) => type === 'Walk')?.value ?? 0
      this.speedText = model.speedText
      // Traits
      this.traitsText = model.traitsText
      this.traits = model.traits.map(trait => ({ ...trait }))
      // People
      this.peopleText = model.peopleText
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
              this.toast('success', `${this.type}.updated`)
            } else {
              const { data } = await createRace(this.payload)
              this.setModel(data)
              this.toast('success', `${this.type}.created`)
            }
            this.$refs.form.reset()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    },
    updateTrait(index, { callback, description, name }) {
      const trait = this.traits[index]
      trait.old = { ...trait }
      trait.description = description
      trait.name = name
      trait.status = 'updated'
      Vue.set(this.traits, index, trait)
      if (callback) {
        callback()
      }
    }
  },
  async created() {
    const id = this.$route.params.id
    if (id && id !== 'new') {
      try {
        const { data } = await getRace(id)
        this.setModel(data)
        this.people = (await getPeople(id)).data
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
