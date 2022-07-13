<template>
  <b-container>
    <h1 v-t="'character.title.step2'" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <form-select id="race" label="character.race.label" :options="raceOptions" placeholder="character.race.placeholder" required v-model="raceId" />
        <template v-if="race">
          <h2 v-t="'character.physical.title'" />
          <h4 v-t="'character.physical.stature.title'" />
          <p v-if="race.sizeText" v-text="race.sizeText" />
          <p>{{ $t('character.physical.size', { size: race.size }) }}</p>
          <form-field
            id="stature"
            label="character.physical.stature.label"
            :minValue="0.01"
            :maxValue="99.99"
            required
            :step="0.01"
            type="number"
            v-model.number="stature"
          >
            <template v-if="race.statureRoll">
              <icon-button icon="dice" text="roll.action" variant="primary" @click="rollStature" />
              <p>{{ $t('roll.format', { roll: race.statureRoll }) }} {{ $t('cm') }}</p>
            </template>
          </form-field>
          <h4 v-t="'character.physical.weight.title'" />
          <p v-if="race.weightText" v-text="race.weightText" />
          <b-row>
            <form-field
              class="col"
              id="weight"
              label="character.physical.weight.label"
              :minValue="0.1"
              :maxValue="999.9"
              required
              :step="0.1"
              type="number"
              v-model.number="weight"
            />
            <form-select
              v-if="weightCategories.length"
              class="col"
              id="weightCategory"
              label="character.physical.weight.category"
              :options="weightCategories"
              v-model="weightCategory"
            >
              <icon-button icon="dice" text="roll.action" variant="primary" @click="rollWeight" />
            </form-select>
          </b-row>
          <h4 v-t="'character.physical.age.title'" />
          <p v-if="race.ageText" v-text="race.ageText" />
          <b-row>
            <form-field class="col" id="age" label="character.physical.age.label" :minValue="1" :maxValue="9999" required type="number" v-model.number="age" />
            <form-select
              v-if="ageCategories.length"
              class="col"
              id="ageCategory"
              label="character.physical.age.category"
              :options="ageCategories"
              v-model="ageCategory"
            >
              <icon-button icon="dice" text="roll.action" variant="primary" @click="rollAge" />
            </form-select>
          </b-row>
          <h2 v-t="'character.name.title'" />
          <p v-if="race.namesText" v-text="race.namesText" />
          <name-field label="character.name.label" placeholder="character.name.placeholder" v-model="name" />
          <template v-if="race.names.length">
            <table class="table table-striped">
              <thead>
                <tr>
                  <th scope="col" v-t="'character.name.category'" />
                  <th scope="col" v-t="'character.name.values'" />
                  <th scope="col" v-t="'character.name.selected'" />
                  <th scope="col" />
                </tr>
              </thead>
              <tbody>
                <tr v-for="nameCategory in race.names" :key="nameCategory.category">
                  <td v-text="nameCategory.category" />
                  <td>
                    <template v-for="(value, index) in nameCategory.values">
                      <a :key="`${value}_val`" href="#" v-text="value" @click.prevent="writeName(value)" />
                      <span v-if="index + 1 < nameCategory.values.length" :key="`${value}_sep`">, </span>
                    </template>
                  </td>
                  <td>{{ names[nameCategory.category] || null }}</td>
                  <td>
                    <icon-button class="mx-1" icon="dice" text="roll.action" variant="primary" @click="rollName(nameCategory)" />
                    <icon-button
                      class="mx-1"
                      :disabled="!names[nameCategory.category]"
                      icon="pen"
                      text="actions.write"
                      variant="success"
                      @click="writeName(names[nameCategory.category], nameCategory.category)"
                    />
                  </td>
                </tr>
              </tbody>
            </table>
            <icon-button icon="dice" text="roll.all" variant="primary" @click="rollNames" />
          </template>
          <h2 v-t="'character.languages.title'" />
          <p v-if="race.languagesText" v-text="race.languagesText" />
          <p v-if="Object.keys(raceLanguages).length">
            {{ $t('character.languages.racial', { languages: orderBy(Object.values(raceLanguages)).join(', ') }) }}
          </p>
          <language-select :disabled="!languageOptions.length" label="character.languages.add" :options="languageOptions" :value="null" @input="addLanguage" />
          <tag-list label="character.languages.selected" :tags="languageTags" @remove="removeLanguage" />
          <p :class="{ 'text-success': remainingLanguages === 0, 'text-danger': remainingLanguages !== 0 }">
            {{ $t('character.languages.remaining', { value: remainingLanguages }) }}
          </p>
          <h2 v-t="'character.bonuses.title'" />
          <div class="my-2">
            <icon-button class="mx-1" icon="plus" text="actions.add" variant="success" v-b-modal.newBonus />
            <bonus-edit-modal id="newBonus" @ok="addBonus" />
          </div>
          <table v-if="bonuses.length" class="table table-striped">
            <thead>
              <tr>
                <th scope="col" v-t="'character.bonuses.types.label'" />
                <th scope="col" v-t="'character.bonuses.target'" />
                <th scope="col" v-t="'description.label'" />
                <th scope="col" v-t="'character.bonuses.value'" />
                <th scope="col" />
              </tr>
            </thead>
            <tbody>
              <tr v-for="(bonus, index) in bonuses" :key="index">
                <td>
                  {{ $t(`character.bonuses.types.${bonus.type}`) }}
                  <bonus-status v-if="bonus.status" :status="bonus.status" />
                </td>
                <td>
                  <template v-if="bonus.type === 'Attribute'">{{ $t(`attribute.options.${bonus.target}`) }}</template>
                  <template v-else-if="bonus.type === 'Skill'">{{ $t(`skill.options.${bonus.target}`) }}</template>
                  <template v-else-if="bonus.type === 'Statistic'">{{ $t(`statistics.options.${bonus.target}`) }}</template>
                  <template v-else-if="bonus.type === 'Other'">{{ $t(`character.bonuses.other.options.${bonus.target}`) }}</template>
                </td>
                <td v-text="shortify(bonus.description, 100)" />
                <td>
                  {{ bonus.value }}
                  <b-badge v-if="bonus.permanent" variant="info">{{ $t('character.bonuses.permanent') }}</b-badge>
                </td>
                <td>
                  <b-button-group>
                    <icon-button v-if="bonus.status === 'removed' || bonus.status === 'updated'" icon="undo" variant="warning" @click="restoreBonus(index)" />
                    <icon-button v-if="bonus.status !== 'removed'" icon="edit" variant="primary" v-b-modal="`editBonus_${index}`" />
                    <icon-button v-if="bonus.status !== 'removed'" icon="times" variant="danger" @click="removeBonus(index)" />
                  </b-button-group>
                  <bonus-edit-modal :bonus="bonus" :id="`editBonus_${index}`" @ok="updateBonus(index, $event)" />
                </td>
              </tr>
            </tbody>
          </table>
          <p v-else v-t="'character.bonuses.empty'" />
          <h4 v-t="'character.bonuses.attributes'" />
          <attribute-select :disabled="remainingAttributes === 0" v-model="attribute">
            <icon-button icon="plus" :disabled="!attribute || remainingAttributes === 0" text="actions.add" variant="success" @click="addAttributeBonus" />
          </attribute-select>
          <p :class="{ 'text-success': remainingAttributes === 0, 'text-danger': remainingAttributes !== 0 }">
            {{ $t('character.attributes.remaining', { value: remainingAttributes }) }}
          </p>
        </template>
        <div class="my-2">
          <icon-button class="mx-1" icon="arrow-left" text="actions.previous" variant="primary" :to="{ name: 'CharacterEditStep1' }" />
          <icon-submit
            class="mx-1"
            :disabled="remainingAttributes !== 0 || remainingLanguages !== 0 || !hasChanges"
            icon="arrow-right"
            text="actions.next"
            variant="primary"
          />
          <icon-button class="mx-1" icon="ban" text="actions.cancel" @click="cancel" />
          <!-- TODO(fpion): confirm cancel -->
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import BonusStatus from './BonusStatus.vue'
import BonusEditModal from './BonusEditModal.vue'
import LanguageSelect from '../Languages/LanguageSelect.vue'
import Vue from 'vue'
import { getLanguages } from '@/api/languages'
import { getRace, getRaces } from '@/api/races'
import { mapActions, mapState } from 'vuex'

export default {
  components: {
    BonusStatus,
    BonusEditModal,
    LanguageSelect
  },
  data: () => ({
    age: 0,
    ageCategory: 'adult',
    attribute: null,
    bonuses: [],
    languages: [],
    loaded: false,
    name: null,
    names: {},
    raceId: null,
    races: [],
    stature: 0,
    weight: 0,
    weightCategory: 'normal',
    worldLanguages: []
  }),
  computed: {
    ...mapState(['characterCreation']),
    ageCategories() {
      return this.race.ageThresholds
        ? [
            { text: `${this.$i18n.t('race.physical.age.child')} (0-${this.race.ageThresholds.teenager - 1})`, value: 'child' },
            {
              text: `${this.$i18n.t('race.physical.age.teenager')} (${this.race.ageThresholds.teenager}-${this.race.ageThresholds.adult - 1})`,
              value: 'teenager'
            },
            { text: `${this.$i18n.t('race.physical.age.adult')} (${this.race.ageThresholds.adult}-${this.race.ageThresholds.mature - 1})`, value: 'adult' },
            {
              text: `${this.$i18n.t('race.physical.age.mature')} (${this.race.ageThresholds.mature}-${this.race.ageThresholds.venerable - 1})`,
              value: 'mature'
            },
            { text: `${this.$i18n.t('race.physical.age.venerable')} (${this.race.ageThresholds.venerable}+)`, value: 'venerable' }
          ]
        : []
    },
    extraAttributeBonuses() {
      return this.bonuses.filter(({ description, status, type }) => type === 'Attribute' && description === this.race?.name && status !== 'removed')
    },
    hasChanges() {
      return this.race !== null
    },
    languageIds() {
      return this.languages.filter(({ status }) => status !== 'removed').map(({ id }) => id)
    },
    languageOptions() {
      return this.worldLanguages
        .filter(({ id }) => !this.raceLanguages[id] && !this.languageIds.includes(id))
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
      return {
        raceId: this.raceId,
        stature: this.stature,
        weight: this.weight,
        weightCategory: this.weightCategory,
        age: this.age,
        ageCategory: this.ageCategory,
        name: this.name,
        languageIds: this.languageIds,
        bonuses: [...this.bonuses]
      }
    },
    race() {
      return this.races.find(({ id }) => id === this.raceId) ?? null
    },
    raceLanguages() {
      return Object.fromEntries(this.race.languages.map(({ id, name }) => [id, name]))
    },
    raceOptions() {
      return this.races.map(({ id, name }) => ({
        text: name,
        value: id
      }))
    },
    remainingAttributes() {
      return (this.race?.extraAttributes ?? 0) - this.extraAttributeBonuses.length
    },
    remainingLanguages() {
      return (this.race?.extraLanguages ?? 0) - this.languages.length
    },
    weightCategories() {
      return this.race.weightRolls
        ? [
            { text: `${this.$i18n.t('race.physical.weight.skinny')} (${this.race.weightRolls.skinny})`, value: 'skinny' },
            { text: `${this.$i18n.t('race.physical.weight.thin')} (${this.race.weightRolls.thin})`, value: 'thin' },
            { text: `${this.$i18n.t('race.physical.weight.normal')} (${this.race.weightRolls.normal})`, value: 'normal' },
            { text: `${this.$i18n.t('race.physical.weight.overweight')} (${this.race.weightRolls.overweight})`, value: 'overweight' },
            { text: `${this.$i18n.t('race.physical.weight.obese')} (${this.race.weightRolls.obese})`, value: 'obese' }
          ]
        : []
    }
  },
  methods: {
    ...mapActions(['resetCharacterCreation', 'saveCharacterCreationStep2']),
    addAttributeBonus() {
      this.bonuses.push({
        type: 'Attribute',
        target: this.attribute,
        description: this.race.name,
        permanent: true,
        value: 1,
        status: 'added'
      })
    },
    addBonus({ callback, description, permanent, target, type, value }) {
      this.bonuses.push({ description, permanent, target, type, value, status: 'added' })
      if (callback) {
        callback()
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
    cancel() {
      this.resetCharacterCreation()
      this.$router.push({ name: 'CharacterList' })
    },
    removeBonus(index) {
      const bonus = this.bonuses[index]
      if (bonus.status === 'added') {
        Vue.delete(this.bonuses, index)
        return
      }
      bonus.status = 'removed'
      Vue.set(this.bonuses, index, bonus)
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
    restoreBonus(index) {
      const bonus = this.bonuses[index]
      if (bonus.old) {
        for (const [key, value] of Object.entries(bonus.old)) {
          bonus[key] = value
        }
        delete bonus.old
      }
      delete bonus.status
      Vue.set(this.bonuses, index, bonus)
    },
    rollAge() {
      let min = 0,
        max = 0
      switch (this.ageCategory) {
        case 'child':
          min = 0
          max = this.race.ageThresholds['teenager']
          break
        case 'teenager':
          min = this.race.ageThresholds[this.ageCategory]
          max = this.race.ageThresholds['adult']
          break
        case 'adult':
          min = this.race.ageThresholds[this.ageCategory]
          max = this.race.ageThresholds['mature']
          break
        case 'mature':
          min = this.race.ageThresholds[this.ageCategory]
          max = this.race.ageThresholds['venerable']
          break
        case 'venerable':
          min = this.race.ageThresholds[this.ageCategory]
          max = 9999 + 1
          break
      }
      if (min && max) {
        this.age = Math.floor(Math.random() * (max - min) + min)
      }
    },
    rollName({ category, values }) {
      const index = Math.floor(Math.random() * values.length)
      Vue.set(this.names, category, values[index])
    },
    rollNames() {
      this.race.names.forEach(nameCategory => this.rollName(nameCategory))
    },
    rollStature() {
      this.stature = this.roll(this.race.statureRoll) / 100
    },
    rollWeight() {
      const imc = this.roll(this.race.weightRolls[this.weightCategory])
      this.weight = Math.round(imc * this.stature * this.stature * 10) / 10
    },
    setModel(model) {
      this.raceId = model.raceId
      this.stature = model.stature
      this.weight = model.weight
      this.weightCategory = model.weightCategory
      this.age = model.age
      this.ageCategory = model.ageCategory
      this.name = model.name
      this.languages = this.worldLanguages.filter(({ id }) => model.languageIds.includes(id)).map(language => ({ ...language, status: 'added' }))
      this.bonuses = [...model.bonuses]
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            alert('OK') // TODO(fpion): implement
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    },
    updateBonus(index, { callback, description, permanent, value }) {
      const bonus = this.bonuses[index]
      bonus.old = { ...bonus }
      bonus.description = description
      bonus.permanent = permanent
      bonus.value = value
      bonus.status = 'updated'
      console.log(bonus)
      Vue.set(this.bonuses, index, bonus)
      if (callback) {
        callback()
      }
    },
    writeName(addedName, category = null) {
      if (category) {
        Vue.delete(this.names, category)
      }
      this.name = this.name ? `${this.name} ${addedName}` : addedName
    }
  },
  async created() {
    try {
      const races = await getRaces({ deleted: false, sort: 'Name' })
      this.races = races.data.items
      const languages = await getLanguages({ deleted: false, sort: 'Name' })
      this.worldLanguages = languages.data.items
      if (this.characterCreation.step2) {
        this.setModel(this.characterCreation.step2)
      }
    } catch (e) {
      this.handleError(e)
    }
    this.loaded = true
  },
  watch: {
    payload: {
      deep: true,
      immediate: true,
      handler(payload) {
        if (this.loaded) {
          this.saveCharacterCreationStep2(payload)
        }
      }
    },
    async raceId(newValue, oldValue) {
      if (newValue !== oldValue) {
        const index = this.races.findIndex(({ id }) => id === newValue)
        if (index >= 0) {
          const race = await getRace(this.races[index].id)
          Vue.set(this.races, index, race.data)
        }
      }
    }
  }
}
</script>
