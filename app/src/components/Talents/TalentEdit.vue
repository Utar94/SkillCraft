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
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('talent.general')" active>
            <b-row>
              <name-field class="col" v-model="name" />
              <tier-select class="col" :disabled="Boolean(talent)" :required="!talent" v-model="tier" />
            </b-row>
            <b-row>
              <talent-select
                class="col"
                :disabled="tier === null"
                :exclude="talent ? [talent.id] : []"
                id="requiredTalentId"
                label="talent.required.label"
                :tiers="[0, 1, 2, 3].filter(value => value <= tier)"
                v-model="requiredTalentId"
              />
              <skill-select class="col" label="talent.skillTraining" v-model="skill" />
            </b-row>
            <b-form-group>
              <b-form-checkbox v-model="multipleAcquisition">{{ $t('talents.multipleAcquisition.label') }}</b-form-checkbox>
            </b-form-group>
            <description-field v-model="description" />
          </b-tab>
          <b-tab :title="$t('talent.options.tab')">
            <div class="my-2">
              <icon-button class="mx-1" icon="plus" text="actions.add" variant="success" v-b-modal.newOption />
              <talent-option-edit-modal id="newOption" @ok="addOption" />
            </div>
            <table v-if="options.length" class="table table-striped">
              <thead>
                <tr>
                  <th scope="col" v-t="'name.label'" />
                  <th scope="col" v-t="'description.label'" />
                  <th scope="col" />
                </tr>
              </thead>
              <tbody>
                <tr v-for="(option, index) in options" :key="index">
                  <td>
                    {{ option.name }}
                    <talent-option-status v-if="option.status" :status="option.status" />
                  </td>
                  <td v-text="shortify(option.description, 100)" />
                  <td>
                    <icon-button
                      class="mx-1"
                      :disabled="option.status !== 'removed' && option.status !== 'updated'"
                      icon="undo"
                      variant="warning"
                      @click="restoreOption(index)"
                    />
                    <icon-button class="mx-1" :disabled="option.status === 'removed'" icon="edit" variant="primary" v-b-modal="`editOption_${index}`" />
                    <icon-button class="mx-1" :disabled="option.status === 'removed'" icon="times" variant="danger" @click="removeOption(index)" />
                  </td>
                  <talent-option-edit-modal :id="`editOption_${index}`" :option="option" @ok="updateOption(index, $event)" />
                </tr>
              </tbody>
            </table>
            <p v-else v-t="'talent.options.empty'" />
          </b-tab>
        </b-tabs>
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
import TalentOptionEditModal from './TalentOptionEditModal.vue'
import TalentOptionStatus from './TalentOptionStatus.vue'
import TalentSelect from './TalentSelect.vue'
import Vue from 'vue'
import { createTalent, getTalent, updateTalent } from '@/api/talents'

export default {
  components: {
    TalentOptionEditModal,
    TalentOptionStatus,
    TalentSelect
  },
  data: () => ({
    description: null,
    loading: false,
    multipleAcquisition: false,
    options: [],
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
        (this.description ?? '') !== (this.talent?.description ?? '') ||
        this.options.some(({ status }) => Boolean(status))
      )
    },
    payload() {
      const payload = {
        description: this.description,
        multipleAcquisition: this.multipleAcquisition,
        name: this.name,
        options: this.options.filter(({ status }) => status !== 'removed').map(({ id, description, name }) => ({ id, description, name })),
        requiredTalentId: this.requiredTalentId,
        skill: this.skill
      }
      if (!this.talent) {
        payload.tier = this.tier
      }
      return payload
    },
    skills() {
      return Object.fromEntries(Object.entries(this.$i18n.t('skill.options')).map(([key, value]) => [value, key]))
    },
    title() {
      return this.talent?.name ?? this.$i18n.t('talent.title')
    }
  },
  methods: {
    addOption({ callback, description, name }) {
      this.options.push({ description, name, status: 'added' })
      if (callback) {
        callback()
      }
    },
    removeOption(index) {
      const option = this.options[index]
      if (option.status === 'added') {
        Vue.delete(this.options, index)
        return
      }
      option.status = 'removed'
      Vue.set(this.options, index, option)
    },
    restoreOption(index) {
      const option = this.options[index]
      if (option.old) {
        for (const [key, value] of Object.entries(option.old)) {
          option[key] = value
        }
        delete option.old
      }
      delete option.status
      Vue.set(this.options, index, option)
    },
    setModel(model) {
      this.talent = model
      this.description = model.description
      this.multipleAcquisition = model.multipleAcquisition
      this.name = model.name
      this.options = model.options.map(option => ({ ...option }))
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
    },
    updateOption(index, { callback, description, name }) {
      const option = this.options[index]
      option.old = { ...option }
      option.description = description
      option.name = name
      option.status = 'updated'
      Vue.set(this.options, index, option)
      if (callback) {
        callback()
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
  },
  watch: {
    name(name) {
      if (this.skill === null) {
        const skill = this.skills[name]
        if (skill) {
          this.skill = skill
        }
      }
    }
  }
}
</script>
